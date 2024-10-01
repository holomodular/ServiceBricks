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
        public const string DEFINITION_FROM_PROPERTY = "FromPropertyName";

        /// <summary>
        /// The key for the to property name.
        /// </summary>
        public const string DEFINITION_TO_PROPERTY = "ToPropertyName";

        /// <summary>
        /// Constructor.
        /// </summary>
        public DomainQueryPropertyRenameRule()
        {
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="fromPropertyName"></param>
        /// <param name="toPropertyName"></param>
        public static void Register(
            IBusinessRuleRegistry registry,
            string fromPropertyName,
            string toPropertyName)
        {
            var custom = new Dictionary<string, object>()
            {
                {DEFINITION_FROM_PROPERTY, fromPropertyName },
                {DEFINITION_TO_PROPERTY, toPropertyName },
            };

            registry.Register(
                typeof(DomainQueryBeforeEvent<TDomainObject>),
                typeof(DomainQueryPropertyRenameRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="fromPropertyName"></param>
        /// <param name="toPropertyName"></param>
        public static void UnRegister(
            IBusinessRuleRegistry registry,
            string fromPropertyName,
            string toPropertyName)
        {
            var custom = new Dictionary<string, object>()
            {
                {DEFINITION_FROM_PROPERTY, fromPropertyName },
                {DEFINITION_TO_PROPERTY, toPropertyName },
            };

            registry.UnRegister(
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var ei = context.Object as DomainQueryBeforeEvent<TDomainObject>;
            if (ei == null || ei.ServiceQueryRequest == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            //Get the property names from the custom context
            if (DefinitionData == null || !DefinitionData.ContainsKey(DEFINITION_FROM_PROPERTY))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            var fpropName = DefinitionData[DEFINITION_FROM_PROPERTY];
            if (fpropName == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            string fromPropertyName = fpropName.ToString();

            if (DefinitionData == null || !DefinitionData.ContainsKey(DEFINITION_TO_PROPERTY))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            var tpropName = DefinitionData[DEFINITION_TO_PROPERTY];
            if (tpropName == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "DefinitionData"));
                return response;
            }
            string toPropertyName = tpropName.ToString();

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

            return response;
        }
    }
}