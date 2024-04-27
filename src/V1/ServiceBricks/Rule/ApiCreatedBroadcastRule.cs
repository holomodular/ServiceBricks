namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a domain object is created via the API.
    /// </summary>
    public partial class ApiCreatedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;

        public ApiCreatedBroadcastRule(
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
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
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            if (context.Object is ApiCreateAfterEvent<TDomain, TDto> createEvent)
            {
                var e = new CreatedBroadcast<TDto>(createEvent.DtoObject);
                _serviceBus.Send(e);
            }
            return new Response();
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            if (context.Object is ApiCreateAfterEvent<TDomain, TDto> createEvent)
            {
                var e = new CreatedBroadcast<TDto>(createEvent.DtoObject);
                await _serviceBus.SendAsync(e);
            }
            return new Response();
        }
    }
}