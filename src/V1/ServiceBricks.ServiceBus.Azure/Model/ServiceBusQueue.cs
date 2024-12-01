using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// This processes service bus messages using a queue.
    /// </summary>
    public partial class ServiceBusQueue : IServiceBus, IAsyncDisposable
    {
        protected readonly ILogger<ServiceBusTopic> _logger;
        protected readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IBusinessRuleRegistry _businessRuleRegistry;
        protected readonly IServiceBusConnection _serviceBusConnection;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceBusQueue"></param>
        /// <param name="businessRuleRegistry"></param>
        /// <param name="serviceBusConnection"></param>
        /// <param name="serviceProvider"></param>
        public ServiceBusQueue(
            ILoggerFactory loggerFactory,
            IServiceBusQueue serviceBusQueue,
            IBusinessRuleRegistry businessRuleRegistry,
            IServiceBusConnection serviceBusConnection,
            IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger<ServiceBusTopic>();
            _serviceBusQueue = serviceBusQueue;
            _businessRuleRegistry = businessRuleRegistry;
            _serviceBusConnection = serviceBusConnection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Start the service bus.
        /// </summary>
        public virtual void Start()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Start the service bus.
        /// </summary>
        /// <returns></returns>
        public virtual async Task StartAsync()
        {
            // Get a list of all queues in service bus
            List<QueueProperties> existingQueues = new List<QueueProperties>();
            var queues = _serviceBusConnection.AdministrationClient.GetQueuesAsync();
            await foreach (var page in queues.AsPages())
                existingQueues.AddRange(page.Values);

            // Find all subscribed events
            var types = _businessRuleRegistry.GetKeys();
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

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var processor in _processors)
                processor.CloseAsync().GetAwaiter().GetResult();
            _processors.Clear();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <returns></returns>
        public virtual async ValueTask DisposeAsync()
        {
            foreach (var processor in _processors)
                await processor.CloseAsync();
            _processors.Clear();
        }

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void Subscribe(Type message, Type handler)
        {
            _businessRuleRegistry.Register(message, handler);
        }

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unsubscribe from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void UnSubscribe(Type message, Type handler)
        {
            _businessRuleRegistry.UnRegister(message, handler);
        }

        /// <summary>
        /// Unsuscribe from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual Task UnSubscribeAsync(Type message, Type handler)
        {
            UnSubscribe(message, handler);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message"></param>
        public virtual void Send(IDomainBroadcast message)
        {
            SendAsync(message).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a message.
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

            var sender = _serviceBusConnection.Client.CreateSender(eventName);
            await sender.SendMessageAsync(msg);
        }

        /// <summary>
        /// Start the queue processor.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        protected virtual async Task StartQueueProcessorAsync(string queueName)
        {
            var processor = _serviceBusConnection.Client.CreateProcessor(queueName);
            _processors.Add(processor);

            processor.ProcessMessageAsync +=
                async (args) =>
                {
                    var eventName = $"{args.Message.Subject}";
                    string messageData = args.Message.Body.ToString();

                    // Process the message
                    if (await ProcessBroadcastAsync(eventName, messageData))
                        await args.CompleteMessageAsync(args.Message);
                    else
                        await args.DeadLetterMessageAsync(args.Message);
                };

            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync();
        }

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"ServiceBus Error: {args.ErrorSource}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Process a broadcast.
        /// </summary>
        /// <param name="broadcastName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task<bool> ProcessBroadcastAsync(string broadcastName, string message)
        {
            // If subscribed to, a reference will be found
            var type = _businessRuleRegistry.GetKeys().Where(x => x.FullName == broadcastName).FirstOrDefault();
            if (type != null)
            {
                // Convert to the broadcast type
                var domainBroadcast = (IDomainBroadcast)JsonConvert.DeserializeObject(message, type);

                using (var scope = _serviceProvider.CreateScope())
                {
                    // Execute the broadcast
                    BusinessRuleContext context = new BusinessRuleContext(domainBroadcast);
                    var businessRuleService = scope.ServiceProvider.GetRequiredService<IBusinessRuleService>();
                    var resp = await businessRuleService.ExecuteRulesAsync(context);
                    return resp.Success;
                }
            }
            return true;
        }
    }
}