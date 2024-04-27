using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work on a queue.
    /// </summary>
    public class ServiceBusQueueHostedService : TaskQueueHostedService
    {
        public ServiceBusQueueHostedService(
            IServiceProvider serviceProvider,
            IServiceBusQueue serviceBusQueue,
            ILoggerFactory logger)
            : base(serviceProvider, serviceBusQueue, logger)
        { }
    }
}