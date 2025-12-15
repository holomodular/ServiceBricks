using ServiceBricks;
using ServiceBricks.TestDataTypes.MongoDb;
//using ServiceBricks.Cache.MongoDb;
//using ServiceBricks.Logging.MongoDb;
//using ServiceBricks.Notification.MongoDb;
//using ServiceBricks.Security.MongoDb;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupMongoDb
    {
        public StartupMongoDb(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksLoggingMongoDb(Configuration);
            //services.AddServiceBricksCacheMongoDb(Configuration);
            //services.AddServiceBricksNotificationMongoDb(Configuration);
            //services.AddServiceBricksSecurityMongoDb(Configuration);
            services.AddServiceBricksTestDataTypesMongoDb(Configuration);
            ProblemDetailsMappingProfile.Register(MapperRegistry.Instance);
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartCustomWebsite(webHostEnvironment);

            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupMongoDb>>();
            logger.LogInformation("Application Started");
        }
    }
}
