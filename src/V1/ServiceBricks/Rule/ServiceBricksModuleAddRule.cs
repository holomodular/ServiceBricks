using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class ServiceBricksModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<ServiceBricksModule>),
                typeof(ServiceBricksModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<ServiceBricksModule>),
                typeof(ServiceBricksModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<ServiceBricksModule>;
            if (e == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Json Serializer
            services.AddSingleton<IJsonSerializer>(JsonSerializer.Instance);

            // Options
            services.AddOptions();
            services.Configure<ApplicationOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_APPLICATIONOPTIONS));
            services.Configure<ApiOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_APIOPTIONS));
            services.Configure<ClientApiOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS));

            // Background task queue for any ordered background processing
            services.AddSingleton<ITaskQueue, TaskQueue>();
            services.AddHostedService<TaskQueueHostedService>();

            // Registries
            services.AddSingleton<IModuleRegistry>(ModuleRegistry.Instance);
            services.AddSingleton<IMapperRegistry>(MapperRegistry.Instance);
            services.AddSingleton<IBusinessRuleRegistry>(BusinessRuleRegistry.Instance);

            // Mappings
            services.AddSingleton<IMapper>(Mapper.Instance);
            DomainTypeMappingProfile.Register(MapperRegistry.Instance);

            // Business Rules
            services.AddScoped<IBusinessRuleService, BusinessRuleService>();

            // Services
            services.AddScoped(typeof(IDomainRepository<>), typeof(DomainRepository<>));
            services.AddScoped<ITimezoneService, TimezoneService>();
            services.AddScoped<IIpAddressService, IpAddressService>();

            // Clients
            services.AddHttpClient();

            // Service Bus
            services.AddSingleton<IServiceBusQueue, ServiceBusQueue>();
            services.AddHostedService<ServiceBusQueueHostedService>();
            services.AddScoped<ServiceBusTaskWorker<IDomainBroadcast>>();
            services.AddSingleton<IServiceBus, ServiceBusInMemory>();

            return response;
        }
    }
}