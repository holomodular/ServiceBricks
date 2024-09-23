using Microsoft.Extensions.Logging;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules
{
    public partial class ExampleDtoRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ExampleDtoRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExampleQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ExampleDto),
                typeof(ExampleDtoRule));
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
                if (context.Object is ExampleDto ei)
                {
                    var item = ei;
                    if (string.IsNullOrEmpty(item.Name))
                        response.AddMessage(ResponseMessage.CreateError("Name must have a value", nameof(ExampleDto.Name)));
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