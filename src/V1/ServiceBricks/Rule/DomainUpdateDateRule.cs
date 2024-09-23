using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have the UpdateDate property.
    /// It ensures that it is populated for create and updates.
    /// </summary>
    public sealed class DomainUpdateDateRule<TDomainObject> : BusinessRule where TDomainObject : IDomainObject<TDomainObject>, IDpUpdateDate
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public DomainUpdateDateRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DomainUpdateDateRule<TDomainObject>>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(DomainUpdateDateRule<TDomainObject>));

            registry.Register(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainUpdateDateRule<TDomainObject>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(DomainUpdateDateRule<TDomainObject>));

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainUpdateDateRule<TDomainObject>));
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
                if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
                {
                    eu.DomainObject.UpdateDate = DateTimeOffset.UtcNow;
                }

                // AI: Make sure the context object is the correct type
                if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
                {
                    ei.DomainObject.UpdateDate = DateTimeOffset.UtcNow;
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