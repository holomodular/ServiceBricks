using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Logging;

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
        public const string Key_PropertyName = "DomainDateTimeOffsetRule_PropertyName";

        private readonly ILogger _logger;
        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public DomainDateTimeOffsetRule(
            ILoggerFactory loggerFactory,
            ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<DomainDateTimeOffsetRule<TDomainObject>>();
            _timezoneService = timezoneService;
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
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
                custom);

            registry.RegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainDateTimeOffsetRule<TDomainObject>),
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
                typeof(DomainDateTimeOffsetRule<TDomainObject>));

            registry.UnRegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(DomainDateTimeOffsetRule<TDomainObject>));
        }

        public void SetProperties(params string[] props)
        {
            if (props == null || props.Length == 0)
                return;
            List<string> list = new List<string>(props);
            if (CustomData.ContainsKey(Key_PropertyName))
                CustomData[Key_PropertyName] = list;
            else
                CustomData.Add(Key_PropertyName, list);
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