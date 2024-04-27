using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    public class ServiceBusTaskWorker<TBroadcast> : ITaskWork<ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast>, ServiceBusTaskWorker<TBroadcast>>
    {
        protected readonly ILogger<ServiceBusTaskWorker<TBroadcast>> _logger;
        protected readonly IServiceBus _serviceBus;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IBusinessRuleService _businessRuleService;

        public ServiceBusTaskWorker(
            ILoggerFactory loggerFactory,
            IServiceBus serviceBus,
            IServiceProvider serviceProvider,
            IBusinessRuleService businessRuleService)
        {
            _logger = loggerFactory.CreateLogger<ServiceBusTaskWorker<TBroadcast>>();
            _serviceBus = serviceBus;
            _serviceProvider = serviceProvider;
            _businessRuleService = businessRuleService;
        }

        public virtual async Task DoWork(ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast> detail, CancellationToken cancellationToken)
        {
            BusinessRuleContext context = new BusinessRuleContext(detail.Message);
            await _businessRuleService.ExecuteRulesAsync(context);
        }
    }
}