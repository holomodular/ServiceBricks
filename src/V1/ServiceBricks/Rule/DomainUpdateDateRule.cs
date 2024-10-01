namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have the UpdateDate property.
    /// It ensures that it is populated for create and updates.
    /// </summary>
    public sealed class DomainUpdateDateRule<TDomainObject> : BusinessRule where TDomainObject : IDomainObject<TDomainObject>, IDpUpdateDate
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DomainUpdateDateRule()
        {
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
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Make sure the context object is the correct type
            if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
            {
                eu.DomainObject.UpdateDate = DateTimeOffset.UtcNow;
                return response;
            }

            // AI: Make sure the context object is the correct type
            if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
            {
                ei.DomainObject.UpdateDate = DateTimeOffset.UtcNow;
                return response;
            }

            response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
            return response;
        }
    }
}