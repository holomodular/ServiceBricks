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

            // Controllers
            services.AddScoped<IExampleApiController, ExampleApiController>();

            // Storage Services
            services.AddScoped<IStorageRepository<ExampleDomain>, ExampleStorageRepository<ExampleDomain>>();

            // Rules
            ExampleQueryRule.Register(BusinessRuleRegistry.Instance);

            // Remove all background tasks/timers for unit testing

            // Register TestManager
            services.AddScoped<ITestManager<ExampleDto>, ExampleTestManager>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}