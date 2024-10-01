using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class ServiceBusAzureModuleQueueAddRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceBusAzureModuleQueueAddRule()
        {
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<ServiceBusAzureModule>),
                typeof(ServiceBusAzureModuleQueueAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<ServiceBusAzureModule>),
                typeof(ServiceBusAzureModuleQueueAddRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleAddEvent<ServiceBusAzureModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;

            // Remove the existing service bus
            var found = services.Where(x => x.ServiceType == typeof(IServiceBus)).FirstOrDefault();
            if (found != null)
                services.Remove(found);

            // Add the new service bus
            services.AddSingleton<IServiceBus, ServiceBusQueue>();
            services.AddSingleton<IServiceBusConnection, ServiceBusConnection>();

            return response;
        }
    }
}