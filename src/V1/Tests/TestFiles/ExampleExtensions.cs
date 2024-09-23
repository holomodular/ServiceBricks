using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ServiceBricks;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model
{
    /// <summary>
    /// IServiceCollection extensions for the Log module.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksExampleInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(ExampleModule), new ExampleModule());

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
            //ExampleDtoRule.Register(BusinessRuleRegistry.Instance);

            return services;
        }

        public static IServiceCollection AddServiceBricksExampleClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Clients
            services.AddScoped<IExampleApiClient, ExampleApiClient>();

            return services;
        }
    }
}