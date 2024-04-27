using Microsoft.EntityFrameworkCore;
using ServiceBricks;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules;
using ServiceBricks.Xunit;
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

            // Add the example module
            services.AddServiceBricksExampleInMemory(Configuration);

            services.AddCustomWebsite(Configuration);

            services.AddServiceBricksComplete();
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