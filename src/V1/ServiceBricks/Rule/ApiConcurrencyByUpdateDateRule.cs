using Microsoft.Extensions.Logging;

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
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApiConcurrencyByUpdateDateRule(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiConcurrencyByUpdateDateRule<TDomainObject, TDto>>();
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(ApiUpdateBeforeEvent<TDomainObject, TDto>),
                typeof(ApiConcurrencyByUpdateDateRule<TDomainObject, TDto>));
        }

        /// <summary>
        /// UnRegister a rule for a domain object.
        /// </summary>
        public static void UnRegisterRule(IBusinessRuleRegistry registry)
        {
            registry.UnRegisterItem(
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

            try
            {
                // AI: Make sure the context object is the correct type
                var e = context.Object as ApiUpdateBeforeEvent<TDomainObject, TDto>;
                if (e == null)
                    return response;

                //Get values
                string propertyName = nameof(e.DomainObject.UpdateDate);
                var dtoProp = e.DtoObject.GetType().GetProperty(propertyName);
                if (dtoProp == null)
                    throw new Exception($"DTO property not found {propertyName}");
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