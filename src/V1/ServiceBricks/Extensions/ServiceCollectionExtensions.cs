using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            // HttpContextAccessor
            services.AddHttpContextAccessor();
            services.AddOptions();

            // Options
            services.Configure<ApplicationOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_APPLICATIONOPTIONS));
            services.Configure<ApiOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_APIOPTIONS));
            services.Configure<ClientApiOptions>(configuration.GetSection(ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS));

            // Background task queue for any ordered background processing
            services.AddSingleton<ITaskQueue, TaskQueue>();
            services.AddHostedService<TaskQueueHostedService>();

            // Business Rules
            services.AddSingleton<IModuleRegistry>(ModuleRegistry.Instance);
            services.AddSingleton<IBusinessRuleRegistry>(BusinessRuleRegistry.Instance);
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
            var provider = services.BuildServiceProvider();
            var registry = provider.GetRequiredService<IModuleRegistry>();
            if (registry == null)
                throw new Exception("You must call AddServiceBricks() prior to calling this method.");

            List<Assembly> assemblies = new List<Assembly>();
            var modules = ModuleRegistry.Instance.GetModules();
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
        public static IServiceCollection AddServiceBricksComplete(this IServiceCollection services)
        {
            // Configure modelstate errors to use response object if needed
            services.Configure<ApiBehaviorOptions>(x => x.InvalidModelStateResponseFactory = context =>
            {
                ObjectResult objectResult = null;
                if (context.HttpContext != null &&
                    context.HttpContext.Request != null &&
                    context.HttpContext.Request.Path.HasValue &&
                    context.HttpContext.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var apiOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApiOptions>>().Value;

                    if (apiOptions.ReturnResponseObject)
                    {
                        Response response = new Response();
                        foreach (var key in context.ModelState.Keys)
                        {
                            foreach (var err in context.ModelState[key].Errors)
                            {
                                if (!string.IsNullOrEmpty(key))
                                    response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage, key));
                                else
                                    response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage));
                            }
                        }
                        objectResult = new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
                        return objectResult;
                    }
                }
                var vpd = new ValidationProblemDetails(context.ModelState);
                objectResult = new ObjectResult(vpd) { StatusCode = StatusCodes.Status400BadRequest };
                return objectResult;
            });

            // Get the automapper assemblies
            List<Assembly> assemblies = new List<Assembly>();
            var modules = ModuleRegistry.Instance.GetModules();
            foreach (var module in modules)
            {
                var tempAssemblies = module.AutomapperAssemblies;
                if (tempAssemblies != null && tempAssemblies.Count > 0)
                {
                    foreach (var tempAssembly in tempAssemblies)
                    {
                        if (!assemblies.Contains(tempAssembly))
                            assemblies.Add(tempAssembly);
                    }
                }
            }

            // Add automapper assemblies
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assemblies);

                // All datetimes as UTC
                cfg.CreateMap<DateTime, DateTime>().ConvertUsing((s, d) =>
                {
                    return DateTime.SpecifyKind(s, DateTimeKind.Utc);
                });
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            return services;
        }
    }
}