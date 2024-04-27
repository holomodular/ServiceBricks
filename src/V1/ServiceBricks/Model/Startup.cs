using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks
{
    public abstract class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        public static event ConfigureServicesCompleteHandler ConfigureServicesCompleteEvent;

        public static event ConfigureCompleteHandler ConfigureCompleteEvent;

        public delegate void ConfigureServicesCompleteHandler(IServiceCollection services);

        public delegate void ConfigureCompleteHandler(IApplicationBuilder app);

        public virtual void AddBricks(IServiceCollection services)
        {
        }

        public virtual void StartBricks(IApplicationBuilder app)
        {
        }

        public virtual void CustomConfigureServices(IServiceCollection services)
        {
            AddBricks(services);

            // Custom event when complete
            if (ConfigureServicesCompleteEvent != null)
                ConfigureServicesCompleteEvent(services);
        }

        public virtual void CustomConfigure(IApplicationBuilder app)
        {
            StartBricks(app);

            // Custom event when complete
            if (ConfigureCompleteEvent != null)
                ConfigureCompleteEvent(app);
        }
    }
}