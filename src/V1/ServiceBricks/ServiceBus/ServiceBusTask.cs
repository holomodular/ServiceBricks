using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a background task that processes messages from the ServiceBusQueue.
    /// </summary>
    public partial class ServiceBusTask<TBroadcast>
        where TBroadcast : IDomainBroadcast
    {
        /// <summary>
        /// The detail of the task.
        /// </summary>
        public class Detail : ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="message"></param>
            public Detail(TBroadcast message) : base(message)
            {
            }
        }

        /// <summary>
        /// The worker for the task.
        /// </summary>
        public class Worker : ServiceBusTaskWorker<TBroadcast>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="loggerFactory"></param>
            /// <param name="serviceBus"></param>
            /// <param name="serviceProvider"></param>
            /// <param name="businessRuleService"></param>
            public Worker(
                ILoggerFactory loggerFactory,
                IServiceBus serviceBus,
                IServiceProvider serviceProvider,
                IBusinessRuleService businessRuleService)
                : base(loggerFactory, serviceBus, serviceProvider, businessRuleService)
            {
            }

            /// <summary>
            /// Do the work.
            /// </summary>
            /// <param name="detail"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task DoWork(Detail detail, CancellationToken cancellationToken)
            {
                var handler = new ServiceBusRuleRegistration<TBroadcast>(detail.Message);
                BusinessRuleContext context = new BusinessRuleContext(handler);
                context.CancellationToken = cancellationToken;
                await _businessRuleService.ExecuteRulesAsync(context);
            }
        }
    }
}