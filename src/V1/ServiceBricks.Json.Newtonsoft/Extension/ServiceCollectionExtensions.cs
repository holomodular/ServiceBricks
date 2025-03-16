using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Json.Newtonsoft
{
    /// <summary>
    /// Extension methods to add Newtonsoft.Json as the default IJsonSerializer.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the Newtonsoft.Json library as the default JSON serializer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksJsonNewtonsoft(this IServiceCollection services, IConfiguration configuration)
        {
            JsonSerializer.Instance = new NewtonsoftJsonSerializer();

            return services;
        }
    }
}