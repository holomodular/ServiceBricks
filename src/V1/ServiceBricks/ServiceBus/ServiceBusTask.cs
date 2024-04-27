using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a background task that processes messages from the ServiceBusQueue.
    /// </summary>
    public class ServiceBusTask<TBroadcast>
        where TBroadcast : IDomainBroadcast
    {
        public class Detail : ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast>
        {
            public Detail(TBroadcast message) : base(message)
            {
            }
        }

        public class Worker : ServiceBusTaskWorker<TBroadcast>
        {
            public Worker(
                ILoggerFactory loggerFactory,
                IServiceBus serviceBus,
                IServiceProvider serviceProvider,
                IBusinessRuleService businessRuleService)
                : base(loggerFactory, serviceBus, serviceProvider, businessRuleService)
            {
            }

            public async Task DoWork(Detail detail, CancellationToken cancellationToken)
            {
                var handler = new ServiceBusRuleRegistration<TBroadcast>(detail.Message);
                BusinessRuleContext context = new BusinessRuleContext(handler);
                var resp = await _businessRuleService.ExecuteRulesAsync(context);
            }
        }
    }
}