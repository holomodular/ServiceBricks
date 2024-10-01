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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        public ApiUpdatedBroadcastRule(
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var updateEvent = context.Object as ApiUpdateAfterEvent<TDomain, TDto>;
            if (updateEvent == null || updateEvent.DtoObject == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // Send the broadcast
            var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
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
            var updateEvent = context.Object as ApiUpdateAfterEvent<TDomain, TDto>;
            if (updateEvent == null || updateEvent.DtoObject == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // Send the broadcast
            var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
            await _serviceBus.SendAsync(e);

            return response;
        }
    }
}