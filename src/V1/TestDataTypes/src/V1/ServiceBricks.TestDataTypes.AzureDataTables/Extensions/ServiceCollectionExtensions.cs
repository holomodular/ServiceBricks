using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// Extensions to add the TestDataTypesAzureDataTables module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesAzureDataTables module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesAzureDataTables(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add parent module
            services.AddServiceBricksTestDataTypes(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesAzureDataTablesModule.Instance);

            // AI: Add module business rules
            TestDataTypesAzureDataTablesModuleAddRule.Register(BusinessRuleRegistry.Instance);
            TestDataTypesAzureDataTablesModuleStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesAzureDataTablesModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
