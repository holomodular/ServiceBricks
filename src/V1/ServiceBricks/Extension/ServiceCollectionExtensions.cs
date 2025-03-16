using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ServiceBricks
{
    /// <summary>
    /// Extensions methods to add the ServiceBricks to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricks(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(ServiceBricksModule.Instance);

            // AI: Add module business rules
            ServiceBricksModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ServiceBricksModuleAddCompleteRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<ServiceBricksModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }

        /// <summary>
        /// Get the ApplicationParts for the ServiceBricks.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<ApplicationPart> GetServiceBricksParts(this IServiceCollection services)
        {
            List<Assembly> assemblies = new List<Assembly>();
            var modules = ModuleRegistry.Instance.GetKeys();
            foreach (var module in modules)
            {
                var tempAssemblies = module.ViewAssemblies;
                if (tempAssemblies != null && tempAssemblies.Count > 0)
                {
                    foreach (var tempAssembly in tempAssemblies)
                    {
                        if (!assemblies.Contains(tempAssembly))
                            assemblies.Add(tempAssembly);
                    }
                }
            }

            List<ApplicationPart> list = new List<ApplicationPart>();
            foreach (var assembly in assemblies)
                list.Add(new AssemblyPart(assembly));
            return list;
        }

        /// <summary>
        /// Finish adding the ServiceBricks to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksComplete(this IServiceCollection services, IConfiguration configuration)
        {
            // Get all modules registered
            var moduleKeys = ModuleRegistry.Instance.GetKeys();

            // Get all business rules
            var businessRuleKeys = BusinessRuleRegistry.Instance.GetKeys();

            // Initial service scope (nothing added)
            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var moduleAddEventTypes = businessRuleKeys.Where(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(ModuleAddEvent<>)).ToList();

                // AI: Fire ModuleAdd event for each module
                foreach (var module in moduleKeys)
                {
                    var moduleType = module.GetType();
                    var moduleAddEventType = moduleAddEventTypes.Where(x => x.GenericTypeArguments.Contains(moduleType)).FirstOrDefault();
                    if (moduleAddEventType != null)
                    {
                        var moduleAddEvent = (ModuleAddEvent)Activator.CreateInstance(moduleAddEventType);
                        moduleAddEvent.DomainObject = module;
                        moduleAddEvent.ServiceCollection = services;
                        moduleAddEvent.Configuration = configuration;

                        // Create business rule service (without logfactory since may not be registered yet)
                        var businessRuleService = new BusinessRuleService(
                            serviceScope.ServiceProvider,
                            BusinessRuleRegistry.Instance);

                        businessRuleService.ExecuteEvent(moduleAddEvent);
                    }
                }
            }

            // Rebuild servicescope (everything added)
            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var moduleAddCompleteEventTypes = businessRuleKeys.Where(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(ModuleAddCompleteEvent<>)).ToList();

                // AI: Execute ModuleAddComplete Event
                foreach (var module in moduleKeys)
                {
                    var moduleType = module.GetType();
                    var moduleAddCompleteEventType = moduleAddCompleteEventTypes.Where(x => x.GenericTypeArguments.Contains(moduleType)).FirstOrDefault();
                    if (moduleAddCompleteEventType != null)
                    {
                        var moduleAddCompleteEvent = (ModuleAddCompleteEvent)Activator.CreateInstance(moduleAddCompleteEventType);
                        moduleAddCompleteEvent.DomainObject = module;
                        moduleAddCompleteEvent.ServiceCollection = services;
                        moduleAddCompleteEvent.Configuration = configuration;

                        // Create business rule service
                        var businessRuleService = serviceScope.ServiceProvider.GetRequiredService<IBusinessRuleService>();
                        businessRuleService.ExecuteEvent(moduleAddCompleteEvent);
                    }
                }

                return services;
            }
        }
    }
}