using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class EntityFrameworkCoreStartup : ServiceBricks.Startup
    {
        public EntityFrameworkCoreStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);

            // Add module
            ModuleRegistry.Instance.Register(new ExampleModule());

            // Register Database
            var builder = new DbContextOptionsBuilder<ExampleInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            services.Configure<DbContextOptions<ExampleInMemoryContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<ExampleInMemoryContext>>(builder.Options);
            services.AddDbContext<ExampleInMemoryContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // API Service
            services.AddScoped<IApiService<ExampleDto>, ExampleApiService>();
            services.AddScoped<IExampleApiService, ExampleApiService>();
            services.AddScoped<IApiService<ExampleWorkProcessDto>, ExampleProcessQueueApiService>();
            services.AddScoped<IExampleProcessQueueApiService, ExampleProcessQueueApiService>();

            // Controllers
            services.AddScoped<IExampleApiController, ExampleApiController>();
            services.AddScoped<IExampleProcessQueueApiController, ExampleProcessQueueApiController>();

            // Storage Services
            services.AddScoped<IStorageRepository<ExampleDomain>, ExampleStorageRepository<ExampleDomain>>();
            services.AddScoped<IStorageRepository<ExampleWorkProcessDomain>, ExampleStorageRepository<ExampleWorkProcessDomain>>();

            // Mappings
            Mapping.ExampleMappingProfile.Register(MapperRegistry.Instance);
            Mapping.ExampleProcessQueueMappingProfile.Register(MapperRegistry.Instance);

            // Rules
            ExampleQueryRule.Register(BusinessRuleRegistry.Instance);

            // Remove all background tasks/timers for unit testing

            // Register TestManager
            services.AddScoped<ITestManager<ExampleDto>, ExampleTestManager>();
            services.AddScoped<ITestManager<ExampleWorkProcessDto>, ExampleProcessQueueTestManager>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}