using ServiceBricks;
using ServiceBricks.TestDataTypes.SqlServer;
using ServiceBricks.Cache.SqlServer;
using ServiceBricks.Logging.SqlServer;
using ServiceBricks.Notification.SqlServer;
using ServiceBricks.Security.SqlServer;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupSqlServer
    {
        public StartupSqlServer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksLoggingSqlServer(Configuration);
            services.AddServiceBricksCacheSqlServer(Configuration);
            services.AddServiceBricksNotificationSqlServer(Configuration);
            services.AddServiceBricksSecuritySqlServer(Configuration);
            services.AddServiceBricksTestDataTypesSqlServer(Configuration);
            ProblemDetailsMappingProfile.Register(MapperRegistry.Instance);
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartCustomWebsite(webHostEnvironment);

            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlServer>>();
            logger.LogInformation("Application Started");
        }
    }
}
