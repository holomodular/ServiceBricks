namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects to determine if a concurrency violation has happened.
    /// It uses the API Update event to check values once returned from the database.
    /// </summary>
    public sealed class ApiConcurrencyByStringRule<TDomainObject, TDto> : BusinessRule
        where TDomainObject : class, IDomainObject<TDomainObject>
        where TDto : class, IDataTransferObject
    {
        /// <summary>
        /// The property name to check for concurrency.
        /// </summary>
        public const string DEFINITION_PROPERTY_KEY = "PropertyName";

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiConcurrencyByStringRule()
        {
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(
            IBusinessRuleRegistry registry,
            string propertyName)
        {
            var custom = new Dictionary<string, object>();
            custom.Add(DEFINITION_PROPERTY_KEY, propertyName);

            registry.Register(
                typeof(ApiUpdateBeforeEvent<TDomainObject, TDto>),
                typeof(ApiConcurrencyByStringRule<TDomainObject, TDto>),
                custom);
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(
            IBusinessRuleRegistry registry,
            string propertyName)
        {
            var custom = new Dictionary<string, object>();
            custom.Add(DEFINITION_PROPERTY_KEY, propertyName);

            registry.UnRegister(
                typeof(ApiUpdateBeforeEvent<TDomainObject, TDto>),
                typeof(ApiConcurrencyByStringRule<TDomainObject, TDto>),
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ApiUpdateBeforeEvent<TDomainObject, TDto>;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // Get the property name from the custom context
            if (DefinitionData == null || !DefinitionData.ContainsKey(DEFINITION_PROPERTY_KEY))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                return response;
            }
            var propName = DefinitionData[DEFINITION_PROPERTY_KEY];
            if (propName == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                return response;
            }
            string propertyName = propName.ToString();

            // Get old and new values
            var existingProp = e.DomainObject.GetType().GetProperty(propertyName).GetValue(e.DomainObject);
            var existingValue = existingProp.ToString();
            var newProp = e.DtoObject.GetType().GetProperty(propertyName).GetValue(e.DtoObject);
            var newValue = newProp.ToString();

            // Concurrency violation check
            if (newValue != existingValue)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE_CONCURRENCY));
                return response;
            }

            return response;
        }
    }
}