using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class ServiceBusAzureModuleStartRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceBusAzureModuleStartRule()
        {
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<ServiceBusAzureModule>),
                typeof(ServiceBusAzureModuleStartRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<ServiceBusAzureModule>),
                typeof(ServiceBusAzureModuleStartRule));
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
            var e = context.Object as ModuleStartEvent<ServiceBusAzureModule>;
            if (e == null || e.ApplicationBuilder == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic

            // Get the service bus and start it
            var serviceBus = e.ApplicationBuilder.ApplicationServices.GetService<IServiceBus>();
            serviceBus.Start();

            return response;
        }
    }
}