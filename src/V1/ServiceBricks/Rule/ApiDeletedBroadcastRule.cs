namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a object is deleted from the API.
    /// </summary>
    public partial class ApiDeletedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;

        public ApiDeletedBroadcastRule(
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        /// <summary>
        /// Register the rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(ApiDeleteAfterEvent<TDomain, TDto>),
                typeof(ApiDeletedBroadcastRule<TDomain, TDto>));
        }

        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            if (context.Object is ApiDeleteAfterEvent<TDomain, TDto> deletedEvent)
            {
                var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
                _serviceBus.Send(e);
            }
            return new Response();
        }

        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            if (context.Object is ApiDeleteAfterEvent<TDomain, TDto> deletedEvent)
            {
                var e = new DeletedBroadcast<TDto>(deletedEvent.DtoObject);
                await _serviceBus.SendAsync(e);
            }
            return new Response();
        }
    }
}