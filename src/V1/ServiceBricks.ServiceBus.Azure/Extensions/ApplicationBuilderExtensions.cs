using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// IApplicationBuilder extensions for the Service Bus Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder StartServiceBricksServiceBusAzureAdvanced(this IApplicationBuilder applicationBuilder)
        {
            var context = applicationBuilder.ApplicationServices.GetService<IServiceBus>();
            context.Start();

            BrickStarted = true;
            return applicationBuilder;
        }

        public static IApplicationBuilder StartServiceBricksServiceBusAzure(this IApplicationBuilder applicationBuilder)
        {
            var context = applicationBuilder.ApplicationServices.GetService<IServiceBus>();
            context.Start();

            BrickStarted = true;
            return applicationBuilder;
        }
    }
}