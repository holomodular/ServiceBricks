﻿using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects to determine if a concurrency violation has happened.
    /// It uses the API Update event to check values once returned from the database.
    /// </summary>
    public partial class ApiConcurrencyByStringRule<TDomainObject, TDto> : BusinessRule
        where TDomainObject : class, IDomainObject<TDomainObject>
        where TDto : class, IDataTransferObject
    {
        public const string Key_PropertyName = "ApiConcurrencyByStringRule_PropertyName";

        protected readonly ILogger _logger;

        protected readonly IStorageRepository<TDomainObject> _storageRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="storageRepository"></param>
        public ApiConcurrencyByStringRule(
            ILoggerFactory loggerFactory,
            IStorageRepository<TDomainObject> storageRepository)
        {
            _logger = loggerFactory.CreateLogger<ApiConcurrencyByStringRule<TDomainObject, TDto>>();
            _storageRepository = storageRepository;
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(
            IBusinessRuleRegistry registry,
            string propertyName)
        {
            var custom = new Dictionary<string, object>();
            custom.Add(Key_PropertyName, propertyName);

            registry.RegisterItem(
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

            try
            {
                var e = context.Object as ApiUpdateBeforeEvent<TDomainObject, TDto>;
                if (e == null)
                    return response;

                var item = e.DomainObject;

                //Get the property name from the custom context
                if (CustomData == null || !CustomData.ContainsKey(Key_PropertyName))
                    throw new Exception("Context missing propertyname");
                var propName = CustomData[Key_PropertyName];
                if (propName == null)
                    throw new Exception("Context propertyname invalid");
                string propertyName = propName.ToString();

                //Get old and new values
                var existingProp = e.DomainObject.GetType().GetProperty(propertyName).GetValue(e.DomainObject);
                var existingValue = existingProp.ToString();
                var newProp = e.DtoObject.GetType().GetProperty(propertyName).GetValue(e.DtoObject);
                var newValue = newProp.ToString();

                //Concurrency violation check
                if (newValue != existingValue)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE_CONCURRENCY));
                    return response;
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