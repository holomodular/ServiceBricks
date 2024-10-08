﻿namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have the CreateDate property.
    /// It ensures that it is populated for create and is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// </summary>
    public sealed class DomainCreateDateRule<TDomainObject> : BusinessRule
        where TDomainObject : IDomainObject<TDomainObject>, IDpCreateDate
    {
        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timezoneService"></param>
        public DomainCreateDateRule(
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
                typeof(DomainCreateDateRule<TDomainObject>));

            registry.Register(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));
        }

        /// <summary>
        /// Execute the business rule.
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
            if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
            {
                var item = ei.DomainObject;
                item.CreateDate = DateTimeOffset.UtcNow;
                return response;
            }

            // AI: Make sure the context object is the correct type
            if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
            {
                var item = eu.DomainObject;

                if (item.CreateDate.Offset != TimeSpan.Zero)
                    item.CreateDate = _timezoneService.ConvertPostBackToUTC(item.CreateDate);
                return response;
            }

            response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
            return response;
        }
    }
}