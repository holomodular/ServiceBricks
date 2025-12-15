using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes.EntityFrameworkCore
{
    /// <summary>
    /// This rule is executed when the TestDataTypesEntityFrameworkCore module is added.
    /// </summary>
    public sealed class TestDataTypesEntityFrameworkCoreModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<TestDataTypesEntityFrameworkCoreModule>),
                typeof(TestDataTypesEntityFrameworkCoreModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<TestDataTypesEntityFrameworkCoreModule>),
                typeof(TestDataTypesEntityFrameworkCoreModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<TestDataTypesEntityFrameworkCoreModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            //var configuration = e.Configuration;

            // AI: Configure all options for the module

            // AI: Add API services for the module. Each DTO should have two registrations, one for the generic IApiService<> and one for the named interface
            
            services.AddScoped<IApiService<TestDto>, TestApiService>();
            services.AddScoped<ITestApiService, TestApiService>();


            // AI: Register mappings
           TestMappingProfile.Register(MapperRegistry.Instance);


            // AI: Register business rules for the module
             DomainCreateUpdateDateRule<Test>.Register(BusinessRuleRegistry.Instance);
            ApiConcurrencyByUpdateDateRule<Test, TestDto>.Register(BusinessRuleRegistry.Instance);
         DomainDateTimeOffsetRule<Test>.Register(BusinessRuleRegistry.Instance,nameof(Test.TestDateTimeOffset));
            DomainQueryPropertyRenameRule<Test>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Key");



            return response;
        }
    }
}
