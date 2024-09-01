using Microsoft.Extensions.Logging;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This is a business rule for domain objects that have a DateTime property.
    /// It ensures the DateTime or DateTime? property is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// Additionally, this rule makes sure that the DateTime property is not less than the minimum date.
    /// </summary>
    public sealed class AzureDataTablesDomainDateTimeRule<TDomainObject> : BusinessRule
        where TDomainObject : class, IDomainObject<TDomainObject>
    {
        /// <summary>
        /// The key for the property name.
        /// </summary>
        public const string Key_PropertyName = "AzureDataTablesDomainDateTimeRule_PropertyName";

        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AzureDataTablesDomainDateTimeRule(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AzureDataTablesDomainDateTimeRule<TDomainObject>>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(
            IBusinessRuleRegistry registry,
            params string[] propertyNames)
        {
            var custom = new Dictionary<string, object>();
            var list = new List<string>(propertyNames);
            custom.Add(Key_PropertyName, list);

            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
                custom);

            registry.RegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegisterRule(
            IBusinessRuleRegistry registry)
        {
            registry.UnRegisterItem(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>));

            registry.UnRegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>));
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
                //Get the property name from the custom context
                if (CustomData == null || !CustomData.ContainsKey(Key_PropertyName))
                    throw new Exception("CustomData missing propertyname");
                var propVal = CustomData[Key_PropertyName];
                if (propVal == null)
                    throw new Exception("CustomData propertyname invalid");
                List<string> propNames = propVal as List<string>;
                if (propNames == null || propNames.Count == 0)
                    throw new Exception("Propertyname list invalid");

                // AI: Make sure the context object is the correct type
                if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
                {
                    var item = ei.DomainObject;
                    List<TDomainObject> curItemList = new List<TDomainObject>() { item };
                    foreach (var propName in propNames)
                    {
                        var curProp = curItemList.AsQueryable().Select(x => x.GetType().GetProperty(propName).GetValue(x)).FirstOrDefault();
                        if (curProp != null)
                        {
                            if (curProp is DateTime DateTime)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (DateTime < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                            }
                            else if (curProp is DateTime?)
                            {
                                DateTime? nullableDateTime = (DateTime?)curProp;
                                if (nullableDateTime.HasValue)
                                {
                                    // AI: Check to make sure date is within bounds for storage
                                    if (nullableDateTime.Value < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                        item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                                }
                            }
                        }
                    }
                }

                // AI: Make sure the context object is the correct type
                if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
                {
                    var item = eu.DomainObject;
                    List<TDomainObject> curItemList = new List<TDomainObject>() { item };
                    foreach (var propName in propNames)
                    {
                        var curProp = curItemList.AsQueryable().Select(x => x.GetType().GetProperty(propName).GetValue(x)).FirstOrDefault();
                        if (curProp != null)
                        {
                            if (curProp is DateTime DateTime)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (DateTime < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                            }
                            else if (curProp is DateTime?)
                            {
                                DateTime? nullableDateTime = (DateTime?)curProp;
                                if (nullableDateTime.HasValue)
                                {
                                    // AI: Check to make sure date is within bounds for storage
                                    if (nullableDateTime.Value < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                        item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                                }
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