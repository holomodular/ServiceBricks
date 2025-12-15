using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// This rule is executed when the TestDataTypesAzureDataTables module is added.
    /// </summary>
    public sealed class TestDataTypesAzureDataTablesModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<TestDataTypesAzureDataTablesModule>),
                typeof(TestDataTypesAzureDataTablesModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<TestDataTypesAzureDataTablesModule>),
                typeof(TestDataTypesAzureDataTablesModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<TestDataTypesAzureDataTablesModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

            // AI: Configure all options for the module

            // AI: Add storage services for the module
            
            services.AddScoped<IStorageRepository<Test>, TestDataTypesStorageRepository<Test>>();


            // AI: Add API services for the module. Each DTO should have two registrations, one for the generic IApiService<> and one for the named interface
            
            services.AddScoped<IApiService<TestDto>, TestApiService>();
            services.AddScoped<ITestApiService, TestApiService>();


            // AI: Register mappings
           TestMappingProfile.Register(MapperRegistry.Instance);


            // AI: Register business rules for the module
            DomainCreateUpdateDateRule<Test>.Register(BusinessRuleRegistry.Instance);
            ApiConcurrencyByUpdateDateRule<Test, TestDto>.Register(BusinessRuleRegistry.Instance);
            AzureDataTablesDomainDateTimeRule<Test>.Register(BusinessRuleRegistry.Instance,nameof(Test.TestDateTime),nameof(Test.TestDateTimeNull),nameof(Test.TestDateTimeNullWithValue),nameof(Test.TestDateTimeNullWithMixedValues),nameof(Test.TestDateOnly),nameof(Test.TestDateOnlyNull),nameof(Test.TestDateOnlyNullWithValue),nameof(Test.TestDateOnlyNullWithMixedValues),nameof(Test.TestTimeOnly),nameof(Test.TestTimeOnlyNull),nameof(Test.TestTimeOnlyNullWithValue),nameof(Test.TestTimeOnlyNullWithMixedValues));
            AzureDataTablesDomainDateTimeOffsetRule<Test>.Register(BusinessRuleRegistry.Instance,nameof(Test.TestDateTimeOffset),nameof(Test.TestDateTimeOffsetNull),nameof(Test.TestDateTimeOffsetNullWithValue),nameof(Test.TestDateTimeOffsetNullWithMixedValues));
            TestCreateRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<Test>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Key");


            return response;
        }
    }
}
