﻿using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have bothe the CreateDate and Update properties.
    /// It ensures that it is populated for create, update and is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// </summary>
    public partial class DomainCreateUpdateDateRule<TDomainObject> : BusinessRule where TDomainObject : IDomainObject<TDomainObject>, IDpCreateDate, IDpUpdateDate
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        private readonly ITimezoneService _timezoneService;

        public DomainCreateUpdateDateRule(
            ILoggerFactory loggerFactory,
            ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<DomainCreateUpdateDateRule<TDomainObject>>();
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
                    typeof(DomainCreateUpdateDateRule<TDomainObject>));

            registry.RegisterItem(
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
            try
            {
                if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
                {
                    var item = eu.DomainObject;
                    if (item != null)
                    {
                        item.UpdateDate = DateTimeOffset.UtcNow;
                        if (item.CreateDate.Offset != TimeSpan.Zero)
                            item.CreateDate = _timezoneService.ConvertPostBackToUTC(item.CreateDate);
                    }
                }
                if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
                {
                    var item = ei.DomainObject;
                    if (item != null)
                    {
                        var now = DateTimeOffset.UtcNow;
                        item.UpdateDate = now;
                        item.CreateDate = now;
                    }
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