using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;
using ServiceBricks.Storage.Postgres;

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// Extensions to add the TestDataTypesPostgres module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesPostgres module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypesEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesPostgresModule.Instance);

            // AI: Add module business rules
            TestDataTypesPostgresModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesPostgresModule>.Register(BusinessRuleRegistry.Instance);
            PostgresDatabaseMigrationRule<TestDataTypesModule, TestDataTypesPostgresContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
