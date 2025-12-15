using ServiceBricks;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupClient
    {
        public StartupClient(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //services.AddServiceBricks(Configuration);
            //services.AddServiceBricksComplete(Configuration);

            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            //app.StartServiceBricks();
            app.StartCustomWebsite(webHostEnvironment);
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupClient>>();
            logger.LogInformation("Application Started");
        }
    }
}