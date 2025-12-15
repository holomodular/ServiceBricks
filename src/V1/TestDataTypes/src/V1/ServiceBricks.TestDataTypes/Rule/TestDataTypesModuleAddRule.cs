using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// This rule is executed when the TestDataTypes module is added.
    /// </summary>
    public sealed class TestDataTypesModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<TestDataTypesModule>),
                typeof(TestDataTypesModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<TestDataTypesModule>),
                typeof(TestDataTypesModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<TestDataTypesModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;

            // AI: Add hosted services for the module

            // AI: Add workers for tasks in the module

            // AI: Configure all options for the module

            // AI: Add API Controllers for each DTO in the module
            
            services.AddScoped<IApiController<TestDto>, TestApiController>();
            services.AddScoped<ITestApiController, TestApiController>();


            // AI: Add any miscellaneous services for the module

            // AI: Register business rules for the module

            return response;
        }
    }
}
