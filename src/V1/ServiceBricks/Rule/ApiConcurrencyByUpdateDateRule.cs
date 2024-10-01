namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects to determine if a concurrency violation has happened.
    /// It uses the API Update event to check values once returned from the database.
    /// </summary>
    public sealed class ApiConcurrencyByUpdateDateRule<TDomainObject, TDto> : BusinessRule
        where TDomainObject : class, IDomainObject<TDomainObject>, IDpUpdateDate
        where TDto : class, IDataTransferObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiConcurrencyByUpdateDateRule()
        {
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ApiUpdateBeforeEvent<TDomainObject, TDto>),
                typeof(ApiConcurrencyByUpdateDateRule<TDomainObject, TDto>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ApiUpdateBeforeEvent<TDomainObject, TDto>),
                typeof(ApiConcurrencyByUpdateDateRule<TDomainObject, TDto>));
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

            //Get values
            string propertyName = nameof(e.DomainObject.UpdateDate);
            var dtoProp = e.DtoObject.GetType().GetProperty(propertyName);
            if (dtoProp == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                return response;
            }

            var dtoVal = dtoProp.GetValue(e.DtoObject);
            if (dtoVal is DateTimeOffset dtoDate)
            {
                //Concurrency violation check
                if (e.DomainObject.UpdateDate != dtoDate)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE_CONCURRENCY));
                    return response;
                }
            }

            return response;
        }
    }
}