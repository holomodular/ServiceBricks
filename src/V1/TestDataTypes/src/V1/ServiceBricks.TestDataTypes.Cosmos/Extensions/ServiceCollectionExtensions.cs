using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Cosmos
{
    /// <summary>
    /// Extensions to add the TestDataTypesCosmos module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesCosmos module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypesEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesCosmosModule.Instance);

            // AI: Add module business rules
            TestDataTypesCosmosModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesCosmosModule>.Register(BusinessRuleRegistry.Instance);
            EntityFrameworkCoreDatabaseEnsureCreatedRule<TestDataTypesModule, TestDataTypesCosmosContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
