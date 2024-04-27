using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Xunit
{
    public class ServiceBrickStartup : ServiceBricks.Startup
    {
        public ServiceBrickStartup(IConfiguration configuration) : base(configuration)
        { }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
        }

        public override void AddBricks(IServiceCollection services)
        {
            base.AddBricks(services);
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            base.StartBricks(app);
        }
    }
}