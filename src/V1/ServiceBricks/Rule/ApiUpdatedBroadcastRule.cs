using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a domain object is created via the API.
    /// </summary>
    public sealed class ApiUpdatedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        /// <param name="loggerFactory"></param>
        public ApiUpdatedBroadcastRule(
            IServiceBus serviceBus,
            ILoggerFactory loggerFactory)
        {
            _serviceBus = serviceBus;
            _logger = loggerFactory.CreateLogger<ApiUpdatedBroadcastRule<TDomain, TDto>>();
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ApiUpdateAfterEvent<TDomain, TDto>),
                typeof(ApiUpdatedBroadcastRule<TDomain, TDto>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ApiUpdateAfterEvent<TDomain, TDto>),
                typeof(ApiUpdatedBroadcastRule<TDomain, TDto>));
        }

        /// <summary>
        /// Execute the rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();
            try
            {
                // AI: Make sure the context object is the correct type
                if (context.Object is ApiUpdateAfterEvent<TDomain, TDto> updateEvent)
                {
                    var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
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
        /// Execute the rule asynchronously.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();
            try
            {
                // AI: Make sure the context object is the correct type
                if (context.Object is ApiUpdateAfterEvent<TDomain, TDto> updateEvent)
                {
                    var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
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