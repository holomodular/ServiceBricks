using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work on a queue.
    /// </summary>
    public partial class ServiceBusQueueHostedService : TaskQueueHostedService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceBusQueue"></param>
        /// <param name="logger"></param>
        public ServiceBusQueueHostedService(
            IServiceProvider serviceProvider,
            IServiceBusQueue serviceBusQueue,
            ILoggerFactory logger)
            : base(serviceProvider, serviceBusQueue, logger)
        { }
    }
}