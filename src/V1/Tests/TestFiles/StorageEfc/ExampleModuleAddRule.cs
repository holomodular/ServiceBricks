using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules;
using ServiceBricks.Xunit;
using Microsoft.EntityFrameworkCore;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class ExampleModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<ExampleModule>),
                typeof(ExampleModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<ExampleModule>),
                typeof(ExampleModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<ExampleModule>;
            if (e == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

            // Register Database
            var builder = new DbContextOptionsBuilder<ExampleInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            services.Configure<DbContextOptions<ExampleInMemoryContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<ExampleInMemoryContext>>(builder.Options);
            services.AddDbContext<ExampleInMemoryContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // API Service
            services.AddScoped<IApiService<ExampleDto>, ExampleApiService>();
            services.AddScoped<IExampleApiService, ExampleApiService>();

            // Controllers
            services.AddScoped<IExampleApiController, ExampleApiController>();

            // Storage Services
            services.AddScoped<IStorageRepository<ExampleDomain>, ExampleStorageRepository<ExampleDomain>>();

            // Mappings
            Mapping.ExampleMappingProfile.Register(MapperRegistry.Instance);
            Mapping.ExampleProcessQueueMappingProfile.Register(MapperRegistry.Instance);

            // Rules
            ExampleQueryRule.Register(BusinessRuleRegistry.Instance);

            return response;
        }
    }
}