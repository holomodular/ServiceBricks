namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have a DateTimeOffset property.
    /// It ensures the datetimeoffset or datetimeoffset? property is always in
    /// UTC offset 0 format, otherwise tries to convert it from the user's timezone.
    /// </summary>
    public sealed class DomainDateTimeOffsetRule<TDomainObject> : BusinessRule
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
        /// <param name="timezoneService"></param>
        public DomainDateTimeOffsetRule(
            ITimezoneService timezoneService)
        {
            _timezoneService = timezoneService;
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
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
                custom);

            registry.Register(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// Unregister a rule
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
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
                custom);

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
                custom);
        }

        public void SetProperties(params string[] props)
        {
            if (props == null || props.Length == 0)
                return;
            List<string> list = new List<string>(props);
            if (DefinitionData.ContainsKey(DEFINITION_PROPERTY_NAMES))
                DefinitionData[DEFINITION_PROPERTY_NAMES] = list;
            else
                DefinitionData.Add(DEFINITION_PROPERTY_NAMES, list);
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
                        // This also gets nullable types
                        if (curProp is DateTimeOffset datetimeoffset)
                        {
                            if (datetimeoffset.Offset != TimeSpan.Zero)
                            {
                                var newval = _timezoneService.ConvertPostBackToUTC(datetimeoffset);
                                item.GetType().GetProperty(propName).SetValue(item, newval);
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
                        // This also gets nullable types
                        if (curProp is DateTimeOffset datetimeoffset)
                        {
                            if (datetimeoffset.Offset != TimeSpan.Zero)
                            {
                                var newval = _timezoneService.ConvertPostBackToUTC(datetimeoffset);
                                item.GetType().GetProperty(propName).SetValue(item, newval);
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