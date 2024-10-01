using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// Extensions for starting ServiceBricks.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Start ServiceBricks
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricks(this IApplicationBuilder applicationBuilder)
        {
            // Get all modules registered
            var moduleKeys = ModuleRegistry.Instance.GetKeys();

            // Get all business rules
            var businessRuleKeys = BusinessRuleRegistry.Instance.GetKeys();

            // Create service scope
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(StartServiceBricks));
                var moduleStartEventTypes = businessRuleKeys.Where(x =>
                x.IsConstructedGenericType &&
                x.GetGenericTypeDefinition() == typeof(ModuleStartEvent<>))
                    .ToList();

                // Order the modules by start priority
                moduleKeys = moduleKeys.OrderBy(x => x.StartPriority).ToList();

                // AI: Fire ModuleStarted event for each module
                foreach (var module in moduleKeys)
                {
                    // Find all registered business rules for this type
                    var moduleType = module.GetType();
                    var moduleStartEventType = moduleStartEventTypes.Where(x => x.GenericTypeArguments.Contains(moduleType)).FirstOrDefault();
                    if (moduleStartEventType != null)
                    {
                        try
                        {
                            // Create the event
                            var moduleStartEvent = (ModuleStartEvent)Activator.CreateInstance(moduleStartEventType);
                            moduleStartEvent.DomainObject = module;
                            moduleStartEvent.ApplicationBuilder = applicationBuilder;

                            // Create business rule service and execute event
                            var businessRuleService = serviceScope.ServiceProvider.GetRequiredService<IBusinessRuleService>();
                            businessRuleService.ExecuteEvent(moduleStartEvent);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, ex.Message);
                        }
                    }
                }
            }

            return applicationBuilder;
        }
    }
}