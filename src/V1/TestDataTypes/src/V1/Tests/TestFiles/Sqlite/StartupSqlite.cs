using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;
using ServiceBricks.TestDataTypes.Sqlite;

namespace ServiceBricks.Xunit
{
    public class StartupSqlite : ServiceBricks.Startup
    {
        public StartupSqlite(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksTestDataTypesSqlite(Configuration);

            // Remove all background tasks/timers for unit testing

            // Register TestManager

        services.AddScoped<ITestManager<TestDto>, TestTestManager>();


            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}
