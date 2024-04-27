using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBricks.ServiceBus.Azure
{
    public class ServiceBusQueue : IServiceBus, IAsyncDisposable
    {
        protected readonly ILogger<ServiceBusTopic> _logger;
        protected readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IBusinessRuleRegistry _domainRuleRegistry;
        protected readonly IServiceBusConnection _serviceBusConnection;
        protected readonly IConfiguration _configuration;
        protected readonly List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();

        public ServiceBusQueue(
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
        }

        public virtual void Start()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        public virtual async Task StartAsync()
        {
            // Get a list of all queues in service bus
            List<QueueProperties> existingQueues = new List<QueueProperties>();
            var queues = _serviceBusConnection.AdministrationClient.GetQueuesAsync();
            await foreach (var page in queues.AsPages())
                existingQueues.AddRange(page.Values);

            // Find all subscribed events
            var types = _domainRuleRegistry.GetKeys();
            var events = types.Where(x =>
                x.IsAssignableTo(typeof(IDomainBroadcast)) && x.IsClass).ToList();
            foreach (var e in events)
            {
                try
                {
                    // Create queue if needed
                    var existing = existingQueues.Where(x => x.Name == e.FullName).FirstOrDefault();
                    if (existing == null)
                        _ = await _serviceBusConnection.AdministrationClient.CreateQueueAsync(e.FullName);

                    // Start processor
                    await StartQueueProcessorAsync(e.FullName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"service bus queue {e.FullName}");
                }
            }
        }

        public virtual void Dispose()
        {
            foreach (var processor in _processors)
                processor.CloseAsync().GetAwaiter().GetResult();
            _processors.Clear();
        }

        public virtual async ValueTask DisposeAsync()
        {
            foreach (var processor in _processors)
                await processor.CloseAsync();
            _processors.Clear();
        }

        public virtual void Subscribe(Type message, Type handler)
        {
            _domainRuleRegistry.RegisterItem(message, handler);
        }

        public virtual Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            return Task.CompletedTask;
        }

        public virtual void Unsubscribe(Type message, Type handler)
        {
            _domainRuleRegistry.UnRegisterItem(message, handler);
        }

        public virtual Task UnsubscribeAsync(Type message, Type handler)
        {
            Unsubscribe(message, handler);
            return Task.CompletedTask;
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

            var sender = _serviceBusConnection.Client.CreateSender(eventName);
            await sender.SendMessageAsync(msg);
        }

        protected virtual async Task StartQueueProcessorAsync(string queueName)
        {
            ServiceBusProcessorOptions options = new ServiceBusProcessorOptions { MaxConcurrentCalls = 10, AutoCompleteMessages = false };
            var processor = _serviceBusConnection.Client.CreateProcessor(queueName, options);
            _processors.Add(processor);

            processor.ProcessMessageAsync +=
                async (args) =>
                {
                    var eventName = $"{args.Message.Subject}";
                    string messageData = args.Message.Body.ToString();

                    // Complete the message so that it is not received again.
                    if (await ProcessBroadcastAsync(eventName, messageData))
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                };

            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync();
        }

        protected virtual Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"{args.ErrorSource}");
            return Task.CompletedTask;
        }

        protected virtual async Task<bool> ProcessBroadcastAsync(string broadcastName, string message)
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