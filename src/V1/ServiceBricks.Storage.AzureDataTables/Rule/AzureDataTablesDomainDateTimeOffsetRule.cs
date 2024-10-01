namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This is a business rule for domain objects that have a DateTimeOffset property.
    /// It ensures the datetimeoffset or datetimeoffset? property is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// Additionally, this rule makes sure that the DateTimeOffset property is not less than the minimum date.
    /// </summary>
    public sealed class AzureDataTablesDomainDateTimeOffsetRule<TDomainObject> : BusinessRule
        where TDomainObject : class, IDomainObject<TDomainObject>
    {
        /// <summary>
        /// The key for the property name.
        /// </summary>
        public const string DEFINITION_PROPERTY_NAMES = "PropertyNames";

        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AzureDataTablesDomainDateTimeOffsetRule(
            ITimezoneService timezoneService)
        {
            _timezoneService = timezoneService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="propertyNames"></param>
        public static void Register(
            IBusinessRuleRegistry registry,
            params string[] propertyNames)
        {
            var custom = new Dictionary<string, object>();
            var list = new List<string>(propertyNames);
            custom.Add(DEFINITION_PROPERTY_NAMES, list);

            registry.Register(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeOffsetRule<TDomainObject>),
                custom);

            registry.Register(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeOffsetRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="propertyNames"></param>
        public static void UnRegister(
            IBusinessRuleRegistry registry,
            params string[] propertyNames)
        {
            var custom = new Dictionary<string, object>();
            var list = new List<string>(propertyNames);
            custom.Add(DEFINITION_PROPERTY_NAMES, list);

            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeOffsetRule<TDomainObject>),
                custom);

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(AzureDataTablesDomainDateTimeOffsetRule<TDomainObject>),
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
                        if (curProp is DateTimeOffset datetimeoffset)
                        {
                            // AI: Check to make sure date is within bounds for storage
                            if (datetimeoffset < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);

                            if (datetimeoffset.Offset != TimeSpan.Zero)
                            {
                                var newval = _timezoneService.ConvertPostBackToUTC(datetimeoffset);

                                if (newval < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);
                                else
                                    item.GetType().GetProperty(propName).SetValue(item, newval);
                            }
                        }
                        else if (curProp is DateTimeOffset?)
                        {
                            DateTimeOffset? nullableDateTimeOffset = (DateTimeOffset?)curProp;
                            if (nullableDateTimeOffset.HasValue)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (nullableDateTimeOffset.Value < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);

                                if (nullableDateTimeOffset.Value.Offset != TimeSpan.Zero)
                                {
                                    DateTimeOffset? newval = _timezoneService.ConvertPostBackToUTC(nullableDateTimeOffset.Value);
                                    if (newval.Value < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                        item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);
                                    else
                                        item.GetType().GetProperty(propName).SetValue(item, newval);
                                }
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
                        if (curProp is DateTimeOffset datetimeoffset)
                        {
                            // AI: Check to make sure date is within bounds for storage
                            if (datetimeoffset < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);

                            if (datetimeoffset.Offset != TimeSpan.Zero)
                            {
                                var newval = _timezoneService.ConvertPostBackToUTC(datetimeoffset);

                                if (newval < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);
                                else
                                    item.GetType().GetProperty(propName).SetValue(item, newval);
                            }
                        }
                        else if (curProp is DateTimeOffset?)
                        {
                            DateTimeOffset? nullableDateTimeOffset = (DateTimeOffset?)curProp;
                            if (nullableDateTimeOffset.HasValue)
                            {
                                // AI: Check to make sure date is within bounds for storage
                                if (nullableDateTimeOffset.Value < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                    item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);

                                if (nullableDateTimeOffset.Value.Offset != TimeSpan.Zero)
                                {
                                    DateTimeOffset? newval = _timezoneService.ConvertPostBackToUTC(nullableDateTimeOffset.Value);
                                    if (newval.Value < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                                        item.GetType().GetProperty(propName).SetValue(item, StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE);
                                    else
                                        item.GetType().GetProperty(propName).SetValue(item, newval);
                                }
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