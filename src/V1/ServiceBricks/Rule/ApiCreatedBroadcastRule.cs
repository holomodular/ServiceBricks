using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a domain object is created via the API.
    /// </summary>
    public sealed class ApiCreatedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        public ApiCreatedBroadcastRule(
            ILoggerFactory loggerFactory,
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
            _logger = loggerFactory.CreateLogger<ApiCreatedBroadcastRule<TDomain, TDto>>();
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(ApiCreateAfterEvent<TDomain, TDto>),
                typeof(ApiCreatedBroadcastRule<TDomain, TDto>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegisterRule(IBusinessRuleRegistry registry)
        {
            registry.UnRegisterItem(
                typeof(ApiCreateAfterEvent<TDomain, TDto>),
                typeof(ApiCreatedBroadcastRule<TDomain, TDto>));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();
            try
            {
                // AI: Make sure the context object is the correct type
                if (context.Object is ApiCreateAfterEvent<TDomain, TDto> createEvent)
                {
                    var e = new CreatedBroadcast<TDto>(createEvent.DtoObject);
                    _serviceBus.Send(e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_BUSINESS_RULE));
            }
            return response;
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();
            try
            {
                // AI: Make sure the context object is the correct type
                if (context.Object is ApiCreateAfterEvent<TDomain, TDto> createEvent)
                {
                    var e = new CreatedBroadcast<TDto>(createEvent.DtoObject);
                    await _serviceBus.SendAsync(e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_BUSINESS_RULE));
            }
            return response;
        }
    }
}