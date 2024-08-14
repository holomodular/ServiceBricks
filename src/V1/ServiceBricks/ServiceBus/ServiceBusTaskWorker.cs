using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    /// <typeparam name="TBroadcast"></typeparam>
    public partial class ServiceBusTaskWorker<TBroadcast> : ITaskWork<ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast>, ServiceBusTaskWorker<TBroadcast>>
    {
        protected readonly ILogger<ServiceBusTaskWorker<TBroadcast>> _logger;
        protected readonly IServiceBus _serviceBus;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IBusinessRuleService _businessRuleService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceBus"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="businessRuleService"></param>
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

        /// <summary>
        /// Do the work.
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task DoWork(ServiceBusTaskDetail<ServiceBusTaskWorker<TBroadcast>, TBroadcast> detail, CancellationToken cancellationToken)
        {
            BusinessRuleContext context = new BusinessRuleContext(detail.Message);
            await _businessRuleService.ExecuteRulesAsync(context);
        }
    }
}