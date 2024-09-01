using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have the CreateDate property.
    /// It ensures that it is populated for create and is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// </summary>
    public sealed class DomainCreateDateRule<TDomainObject> : BusinessRule
        where TDomainObject : IDomainObject<TDomainObject>, IDpCreateDate
    {
        private readonly ILogger _logger;
        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public DomainCreateDateRule(
            ILoggerFactory loggerFactory,
            ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<DomainCreateDateRule<TDomainObject>>();
            _timezoneService = timezoneService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));

            registry.RegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegisterRule(IBusinessRuleRegistry registry)
        {
            registry.UnRegisterItem(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(DomainCreateDateRule<TDomainObject>));

            registry.UnRegisterItem(
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

            try
            {
                // AI: Make sure the context object is the correct type
                if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
                {
                    var item = ei.DomainObject;
                    item.CreateDate = DateTimeOffset.UtcNow;
                }

                // AI: Make sure the context object is the correct type
                if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
                {
                    var item = eu.DomainObject;

                    if (item.CreateDate.Offset != TimeSpan.Zero)
                        item.CreateDate = _timezoneService.ConvertPostBackToUTC(item.CreateDate);
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