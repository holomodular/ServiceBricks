using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects when querying to rename a
    /// property.
    /// </summary>
    public sealed class DomainQueryPropertyRenameRule<TDomainObject> : BusinessRule
        where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// The key for the from property name.
        /// </summary>
        public const string Key_FromPropertyName = "DomainQueryPropertyRenameRule_FromPropertyName";

        /// <summary>
        /// The key for the to property name.
        /// </summary>
        public const string Key_ToPropertyName = "DomainQueryPropertyRenameRule_ToPropertyName";

        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public DomainQueryPropertyRenameRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DomainQueryPropertyRenameRule<TDomainObject>>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(
            IBusinessRuleRegistry registry,
            string fromPropertyName,
            string toPropertyName)
        {
            var custom = new Dictionary<string, object>()
            {
                {Key_FromPropertyName, fromPropertyName },
                {Key_ToPropertyName, toPropertyName },
            };

            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<TDomainObject>),
                typeof(DomainQueryPropertyRenameRule<TDomainObject>),
                custom);
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
                if (context.Object is DomainQueryBeforeEvent<TDomainObject> ei)
                {
                    //Get the property names from the custom context
                    if (CustomData == null || !CustomData.ContainsKey(Key_FromPropertyName))
                        throw new Exception("CustomData missing from propertyname");
                    var fpropName = CustomData[Key_FromPropertyName];
                    if (fpropName == null)
                        throw new Exception("CustomData frompropertyname invalid");
                    string fromPropertyName = fpropName.ToString();

                    if (CustomData == null || !CustomData.ContainsKey(Key_ToPropertyName))
                        throw new Exception("CustomData missing to propertyname");
                    var tpropName = CustomData[Key_ToPropertyName];
                    if (tpropName == null)
                        throw new Exception("CustomData topropertyname invalid");
                    string toPropertyName = tpropName.ToString();

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
                                if (string.Compare(filter.Properties[i], fromPropertyName, true) == 0)
                                    filter.Properties[i] = toPropertyName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }
    }
}