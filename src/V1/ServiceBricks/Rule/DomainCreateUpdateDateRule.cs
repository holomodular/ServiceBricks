namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have bothe the CreateDate and Update properties.
    /// It ensures that it is populated for create, update and is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// </summary>
    public sealed class DomainCreateUpdateDateRule<TDomainObject> : BusinessRule where TDomainObject : IDomainObject<TDomainObject>, IDpCreateDate, IDpUpdateDate
    {
        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timezoneService"></param>
        public DomainCreateUpdateDateRule(
            ITimezoneService timezoneService)
        {
            _timezoneService = timezoneService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                    typeof(DomainCreateBeforeEvent<TDomainObject>),
                    typeof(DomainCreateUpdateDateRule<TDomainObject>));

            registry.Register(
                    typeof(DomainUpdateBeforeEvent<TDomainObject>),
                    typeof(DomainCreateUpdateDateRule<TDomainObject>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                    typeof(DomainCreateBeforeEvent<TDomainObject>),
                    typeof(DomainCreateUpdateDateRule<TDomainObject>));

            registry.UnRegister(
                    typeof(DomainUpdateBeforeEvent<TDomainObject>),
                    typeof(DomainCreateUpdateDateRule<TDomainObject>));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Make sure the context object is the correct type
            if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
            {
                if (eu.DomainObject == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }

                var item = eu.DomainObject;
                item.UpdateDate = DateTimeOffset.UtcNow;
                if (item.CreateDate.Offset != TimeSpan.Zero)
                    item.CreateDate = _timezoneService.ConvertPostBackToUTC(item.CreateDate);
                return response;
            }

            // AI: Make sure the context object is the correct type
            if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
            {
                if (ei.DomainObject == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }

                var item = ei.DomainObject;
                var now = DateTimeOffset.UtcNow;
                item.UpdateDate = now;
                item.CreateDate = now;

                return response;
            }

            response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
            return response;
        }
    }
}