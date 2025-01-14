//using Microsoft.AspNetCore.Hosting;
//using ServiceBricks;
//using System.Configuration;
//using WebApp.Extensions;

//namespace WebApp
//{
//    public class StartupMongoDb
//    {
//        public StartupMongoDb(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public virtual IConfiguration Configuration { get; set; }

//        public virtual void ConfigureServices(IServiceCollection services)
//        {
//            services.AddServiceBricks(Configuration);
//            services.AddCustomWebsite(Configuration);
//            services.AddServiceBricksComplete();
//        }

//        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
//        {
//            app.StartServiceBricks();
//            app.StartCustomWebsite(webHostEnvironment);
//            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupMongoDb>>();
//            logger.LogInformation("Application Started");
//        }
//    }
//}