using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// Extensions for adding the Test All DataTypes Microservice.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypes module clients to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesClient(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add clients for the module for each DTO
            
            services.AddScoped<IApiClient<TestDto>, TestApiClient>();
            services.AddScoped<ITestApiClient, TestApiClient>();


            return services;
        }
    }
}
