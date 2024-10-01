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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        public ApiDeletedBroadcastRule(
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var deletedEvent = context.Object as ApiDeleteAfterEvent<TDomain, TDto>;
            if (deletedEvent == null || deletedEvent.DtoObject == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // Send the broadcast
            var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
            _serviceBus.Send(e);

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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var deletedEvent = context.Object as ApiDeleteAfterEvent<TDomain, TDto>;
            if (deletedEvent == null || deletedEvent.DtoObject == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
            await _serviceBus.SendAsync(e);

            return response;
        }
    }
}