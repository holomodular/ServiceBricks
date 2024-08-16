using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// Extension methods to start the ServiceBricks ServiceBus Azure module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has been started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks ServiceBus Azure module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksServiceBusAzureQueue(this IApplicationBuilder applicationBuilder)
        {
            // Get the service bus and start it
            var context = applicationBuilder.ApplicationServices.GetService<IServiceBus>();
            context.Start();

            // Set the module started flag.
            ModuleStarted = true;

            return applicationBuilder;
        }

        /// <summary>
        /// Start the ServiceBricks ServiceBus Azure module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksServiceBusAzureTopic(this IApplicationBuilder applicationBuilder)
        {
            // Get the service bus and start it
            var context = applicationBuilder.ApplicationServices.GetService<IServiceBus>();
            context.Start();

            // Set the module started flag.
            ModuleStarted = true;

            return applicationBuilder;
        }
    }
}