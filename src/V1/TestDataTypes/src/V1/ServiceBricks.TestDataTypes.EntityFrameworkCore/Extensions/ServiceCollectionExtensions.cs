using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes.EntityFrameworkCore
{
    /// <summary>
    /// Extensions to add the TestDataTypesEntityFrameworkCore module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesEntityFrameworkCore module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypes(configuration);

            // AI: Add this module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesEntityFrameworkCoreModule.Instance);

            // AI: Add module business rules
            TestDataTypesEntityFrameworkCoreModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesEntityFrameworkCoreModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
