using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// This processes service bus messages using topics
    /// </summary>
    public partial class ServiceBusTopic : IServiceBus, IAsyncDisposable
    {
        protected readonly ILogger<ServiceBusTopic> _logger;
        protected readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IBusinessRuleRegistry _domainRuleRegistry;
        protected readonly IServiceBusConnection _serviceBusConnection;
        protected readonly IConfiguration _configuration;

        protected ServiceBusProcessor _processor;

        /// <summary>
        /// The default topic name
        /// </summary>
        public const string DEFAULT_TOPIC_NAME = "ServiceBricksTopic";

        /// <summary>
        /// The default subscription name
        /// </summary>
        public const string DEFAULT_SUBSCRIPTION_NAME = "ServiceBricksSubscription";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceBusQueue"></param>
        /// <param name="domainRuleRegistry"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceBusConnection"></param>
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

            // Set topic
            var topic = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_TOPIC);
            if (string.IsNullOrEmpty(topic))
                topic = DEFAULT_TOPIC_NAME;
            Topic = topic;

            // Set subscription
            var subscription = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_SUBSCRIPTION);
            if (string.IsNullOrEmpty(subscription))
                subscription = DEFAULT_SUBSCRIPTION_NAME;
            Subscription = subscription;
        }

        /// <summary>
        /// The topic name
        /// </summary>
        public virtual string Topic { get; set; }

        /// <summary>
        /// Subscription name
        /// </summary>
        public virtual string Subscription { get; set; }

        /// <summary>
        /// Start the service bus
        /// </summary>
        public virtual void Start()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Start the service bus
        /// </summary>
        /// <returns></returns>
        public virtual async Task StartAsync()
        {
            // Create Topic and Subscription
            await CreateTopicSubscriptionAsync();

            // Get a list of all rules in service bus
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

            // Start processor
            _processor = _serviceBusConnection.Client.CreateProcessor(Topic, Subscription);
            await StartTopicProcessorAsync();
        }

        /// <summary>
        /// Get the rule name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual string GetRuleName(Type type)
        {
            string name = type.FullName;
            if (name.Length > 50)
            {
                int startIndex = name.Length - 50;
                return name.Substring(startIndex, 50);
            }
            return name;
        }

        /// <summary>
        /// Get the list of rules
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create a rule
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create the topic and subscription
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            _processor?.CloseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <returns></returns>
        public virtual async ValueTask DisposeAsync()
        {
            await _processor?.CloseAsync();
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message"></param>
        public virtual void Send(IDomainBroadcast message)
        {
            SendAsync(message).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Subscribe to a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void Subscribe(Type message, Type handler)
        {
            _domainRuleRegistry.RegisterItem(message, handler);
        }

        /// <summary>
        /// Subscribe to a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual async Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Unsuscribe from a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void Unsubscribe(Type message, Type handler)
        {
            UnsubscribeAsync(message, handler).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Unsuscribe from a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove the default rule
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Start topic processor
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Handle error
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"{args.ErrorSource}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Process a broadcast
        /// </summary>
        /// <param name="broadcastName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
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