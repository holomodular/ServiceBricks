using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBricks.ServiceBus.Azure
{
    public class ServiceBusTopic : IServiceBus, IAsyncDisposable
    {
        public const string DEFAULT_TOPIC_NAME = "ServiceBricksTopic";
        public const string DEFAULT_SUBSCRIPTION_NAME = "ServiceBricksSubscription";

        protected readonly ILogger<ServiceBusTopic> _logger;
        protected readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IBusinessRuleRegistry _domainRuleRegistry;
        protected readonly IServiceBusConnection _serviceBusConnection;
        protected readonly IConfiguration _configuration;

        protected ServiceBusProcessor _processor;

        public ServiceBusTopic(
            ILoggerFactory loggerFactory,
            IServiceBusQueue serviceBusQueue,
            IBusinessRuleRegistry domainRuleRegistry,
            IConfiguration configuration,
            IServiceBusConnection serviceBusConnection)
        {
            _logger = loggerFactory.CreateLogger<ServiceBusTopic>();
            _serviceBusQueue = serviceBusQueue;
            _domainRuleRegistry = domainRuleRegistry;
            _configuration = configuration;
            _serviceBusConnection = serviceBusConnection;

            var topic = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_TOPIC);
            if (string.IsNullOrEmpty(topic))
                topic = DEFAULT_TOPIC_NAME;
            Topic = topic;

            var subscription = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_SUBSCRIPTION);
            if (string.IsNullOrEmpty(subscription))
                subscription = DEFAULT_SUBSCRIPTION_NAME;
            Subscription = subscription;
        }

        public virtual string Topic { get; set; }
        public virtual string Subscription { get; set; }

        public virtual void Start()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        public virtual async Task StartAsync()
        {
            // Create Topic and Subscription
            await CreateTopicSubscriptionAsync();

            var existingRules = await GetRules();
            // Find all subscribed events
            var types = _domainRuleRegistry.GetKeys();
            var events = types.Where(x =>
                x.IsAssignableTo(typeof(IDomainBroadcast)) && x.IsClass).ToList();
            foreach (var e in events)
            {
                // Create rule if needed
                string maxRuleName = GetRuleName(e);
                var existing = existingRules.Where(x => x.Name == maxRuleName).FirstOrDefault();
                if (existing == null)
                    await CreateRule(e);
            }

            // Remove default "true" rule for subscription
            if (existingRules.Where(x => x.Name == RuleProperties.DefaultRuleName).Any())
                await RemoveDefaultRuleAsync();

            ServiceBusProcessorOptions options = new ServiceBusProcessorOptions { MaxConcurrentCalls = 10, AutoCompleteMessages = false };
            _processor = _serviceBusConnection.Client.CreateProcessor(Topic, Subscription, options);
            await StartTopicProcessorAsync();
        }

        protected string GetRuleName(Type type)
        {
            string name = type.FullName;
            if (name.Length > 50)
            {
                int startIndex = name.Length - 50;
                return name.Substring(startIndex, 50);
            }
            return name;
        }

        protected virtual async Task<List<RuleProperties>> GetRules()
        {
            try
            {
                List<RuleProperties> list = new List<RuleProperties>();
                var respRules = _serviceBusConnection.
                        AdministrationClient.GetRulesAsync(Topic, Subscription);

                await foreach (var page in respRules)
                    list.Add(page);
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ServiceBusTopic)} {nameof(GetRules)}");
                return new List<RuleProperties>();
            }
        }

        protected virtual async Task CreateRule(Type message)
        {
            try
            {
                string maxRuleName = GetRuleName(message);
                await _serviceBusConnection.
                    AdministrationClient.
                    CreateRuleAsync(Topic, Subscription, new CreateRuleOptions
                    {
                        Filter = new CorrelationRuleFilter() { Subject = message.FullName },
                        Name = maxRuleName
                    });
            }
            catch (Exception ex)
            {
                if (ex is ServiceBusException sbe)
                    if (sbe.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
                        return;
                _logger.LogError(ex, $"Subscribing to {message.FullName} subscription");
            }
        }

        protected virtual async Task CreateTopicSubscriptionAsync()
        {
            try
            {
                var respExists = await _serviceBusConnection.AdministrationClient.TopicExistsAsync(Topic);
                if (!respExists.Value)
                    await _serviceBusConnection.AdministrationClient.CreateTopicAsync(Topic);

                respExists = await _serviceBusConnection.AdministrationClient.SubscriptionExistsAsync(Topic, Subscription);
                if (!respExists.Value)
                    await _serviceBusConnection.AdministrationClient.CreateSubscriptionAsync(Topic, Subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateTopicSubscription {Topic} {Subscription}");
            }
        }

        public virtual void Dispose()
        {
            _processor?.CloseAsync().GetAwaiter().GetResult();
        }

        public virtual async ValueTask DisposeAsync()
        {
            await _processor?.CloseAsync();
        }

        public virtual void Send(IDomainBroadcast message)
        {
            SendAsync(message).GetAwaiter().GetResult();
        }

        public virtual async Task SendAsync(IDomainBroadcast message)
        {
            var eventName = message.GetType().FullName;
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var msg = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = new BinaryData(body),
                Subject = eventName,
            };

            var sender = _serviceBusConnection.Client.CreateSender(Topic);
            await sender.SendMessageAsync(msg);
        }

        public virtual void Subscribe(Type message, Type handler)
        {
            _domainRuleRegistry.RegisterItem(message, handler);
        }

        public virtual async Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            await Task.CompletedTask;
        }

        public virtual void Unsubscribe(Type message, Type handler)
        {
            UnsubscribeAsync(message, handler).GetAwaiter().GetResult();
        }

        public virtual async Task UnsubscribeAsync(Type message, Type handler)
        {
            _domainRuleRegistry.UnRegisterItem(message, handler);

            try
            {
                await _serviceBusConnection
                    .AdministrationClient
                    .DeleteRuleAsync(Topic, Subscription, message.FullName);
            }
            catch (Exception ex)
            {
                if (ex is ServiceBusException sbe)
                    if (sbe.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
                        return;
                _logger.LogError(ex, $"Unsubscribing from {message.FullName} subscription");
            }
        }

        protected virtual async Task RemoveDefaultRuleAsync()
        {
            try
            {
                await _serviceBusConnection
                    .AdministrationClient
                    .DeleteRuleAsync(Topic, Subscription, RuleProperties.DefaultRuleName);
            }
            catch (Exception ex)
            {
                if (ex is ServiceBusException sbe)
                    if (sbe.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
                        return;
                _logger.LogError(ex, "RemoveDefaultRule");
            }
        }

        protected virtual async Task StartTopicProcessorAsync()
        {
            _processor.ProcessMessageAsync +=
                async (args) =>
                {
                    var eventName = $"{args.Message.Subject}";
                    string messageData = args.Message.Body.ToString();

                    // Complete the message so that it is not received again.
                    if (await ProcessBroadcast(eventName, messageData))
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                };

            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        protected virtual Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"{args.ErrorSource}");
            return Task.CompletedTask;
        }

        protected virtual async Task<bool> ProcessBroadcast(string broadcastName, string message)
        {
            var processed = false;

            // If subscribed to, a reference will be found
            var type = _domainRuleRegistry.GetKeys().Where(x => x.FullName == broadcastName).FirstOrDefault();
            if (type != null)
            {
                var domainBroadcast = (IDomainBroadcast)JsonConvert.DeserializeObject(message, type);
                var detail = new ServiceBusTask<IDomainBroadcast>.Detail(domainBroadcast);
                _serviceBusQueue.Queue(detail);
            }
            processed = true;
            return await Task.FromResult<bool>(processed);
        }
    }
}