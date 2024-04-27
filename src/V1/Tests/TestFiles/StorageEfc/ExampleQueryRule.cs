using Microsoft.Extensions.Logging;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules
{
    public partial class ExampleQueryRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ExampleQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExampleQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<ExampleDomain>),
                typeof(ExampleQueryRule));
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
                if (context.Object is DomainQueryBeforeEvent<ExampleDomain> ei)
                {
                    var item = ei.DomainObject;
                    if (ei.ServiceQueryRequest == null || ei.ServiceQueryRequest.Filters == null)
                        return response;
                    foreach (var filter in ei.ServiceQueryRequest.Filters)
                    {
                        if (filter.Properties != null &&
                            filter.Properties.Count > 0)
                        {
                            for (int i = 0; i < filter.Properties.Count; i++)
                            {
                                if (string.Compare(filter.Properties[i], "StorageKey", true) == 0)
                                    filter.Properties[i] = "Key";
                            }
                        }
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