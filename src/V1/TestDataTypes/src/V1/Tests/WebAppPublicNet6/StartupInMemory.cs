using ServiceBricks;
using ServiceBricks.TestDataTypes.InMemory;
//using ServiceBricks.Cache.InMemory;
//using ServiceBricks.Logging.InMemory;
//using ServiceBricks.Notification.InMemory;
//using ServiceBricks.Security.InMemory;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupInMemory
    {
        public StartupInMemory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksLoggingInMemory(Configuration);
            //services.AddServiceBricksCacheInMemory(Configuration);
            //services.AddServiceBricksNotificationInMemory(Configuration);
            //services.AddServiceBricksSecurityInMemory(Configuration);
            services.AddServiceBricksTestDataTypesInMemory(Configuration);
            ProblemDetailsMappingProfile.Register(MapperRegistry.Instance);
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartCustomWebsite(webHostEnvironment);

            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupInMemory>>();
            logger.LogInformation("Application Started");
        }
    }
}
