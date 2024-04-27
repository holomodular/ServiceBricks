namespace ServiceBricks
{
    /// <summary>
    /// This is a domain rule that handles sending a service bus message
    /// when a domain object is created via the API.
    /// </summary>
    public partial class ApiUpdatedBroadcastRule<TDomain, TDto> : BusinessRule
        where TDomain : IDomainObject<TDomain>
    {
        private readonly IServiceBus _serviceBus;

        public ApiUpdatedBroadcastRule(
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
                typeof(ApiUpdateAfterEvent<TDomain, TDto>),
                typeof(ApiUpdatedBroadcastRule<TDomain, TDto>));
        }

        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            if (context.Object is ApiUpdateAfterEvent<TDomain, TDto> updateEvent)
            {
                var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
                _serviceBus.Send(e);
            }
            return new Response();
        }

        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            if (context.Object is ApiUpdateAfterEvent<TDomain, TDto> updateEvent)
            {
                var e = new UpdatedBroadcast<TDto>(updateEvent.DtoObject);
                await _serviceBus.SendAsync(e);
            }
            return new Response();
        }
    }
}