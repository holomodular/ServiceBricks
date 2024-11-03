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
        public const string DEFINITION_PROPERTY_NAMES = "PropertyNames";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AzureDataTablesDomainDateTimeRule()
        {
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(
            IBusinessRuleRegistry registry,
            params string[] propertyNames)
        {
            var custom = new Dictionary<string, object>();
            var list = new List<string>(propertyNames);
            custom.Add(DEFINITION_PROPERTY_NAMES, list);

            registry.Register(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
                custom);

            registry.Register(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(
            IBusinessRuleRegistry registry,
            params string[] propertyNames)
        {
            var custom = new Dictionary<string, object>();
            var list = new List<string>(propertyNames);
            custom.Add(DEFINITION_PROPERTY_NAMES, list);

            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
                custom);

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeRule<TDomainObject>),
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
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            //Get the property name from the custom context
            if (DefinitionData == null || !DefinitionData.ContainsKey(DEFINITION_PROPERTY_NAMES))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            var propVal = DefinitionData[DEFINITION_PROPERTY_NAMES];
            if (propVal == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            List<string> propNames = propVal as List<string>;
            if (propNames == null || propNames.Count == 0)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }

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
                            else if (DateTime.Kind != DateTimeKind.Utc)
                                item.GetType().GetProperty(propName).SetValue(item, DateTime.SpecifyKind(DateTime, DateTimeKind.Utc));
                        }
                        else if (curProp is DateTime?)
                        {
                            DateTime? nullableDateTime = (DateTime?)curProp;
                            if (nullableDateTime.HasValue)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (nullableDateTime.Value < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                                else if (nullableDateTime.Value.Kind != DateTimeKind.Utc)
                                    item.GetType().GetProperty(propName).SetValue(item, DateTime.SpecifyKind(nullableDateTime.Value, DateTimeKind.Utc));
                            }
                        }
                    }
                }
                return response;
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
                            else if (DateTime.Kind != DateTimeKind.Utc)
                                item.GetType().GetProperty(propName).SetValue(item, DateTime.SpecifyKind(DateTime, DateTimeKind.Utc));
                        }
                        else if (curProp is DateTime?)
                        {
                            DateTime? nullableDateTime = (DateTime?)curProp;
                            if (nullableDateTime.HasValue)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (nullableDateTime.Value < StorageAzureDataTablesConstants.DATETIME_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIME_MINDATE);
                                else if (nullableDateTime.Value.Kind != DateTimeKind.Utc)
                                    item.GetType().GetProperty(propName).SetValue(item, DateTime.SpecifyKind(nullableDateTime.Value, DateTimeKind.Utc));
                            }
                        }
                    }
                }
                return response;
            }

            response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
            return response;
        }
    }
}