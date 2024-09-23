using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a object is deleted from the API.
    /// </summary>
    public sealed class ApiDeletedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        /// <param name="loggerFactory"></param>
        public ApiDeletedBroadcastRule(
            IServiceBus serviceBus,
            ILoggerFactory loggerFactory)
        {
            _serviceBus = serviceBus;
            _logger = loggerFactory.CreateLogger<ApiDeletedBroadcastRule<TDomain, TDto>>();
        }

        /// <summary>
        /// Register the rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ApiDeleteAfterEvent<TDomain, TDto>),
                typeof(ApiDeletedBroadcastRule<TDomain, TDto>));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ApiDeleteAfterEvent<TDomain, TDto>),
                typeof(ApiDeletedBroadcastRule<TDomain, TDto>));
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
                if (context.Object is ApiDeleteAfterEvent<TDomain, TDto> deletedEvent)
                {
                    var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
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
                if (context.Object is ApiDeleteAfterEvent<TDomain, TDto> deletedEvent)
                {
                    var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
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