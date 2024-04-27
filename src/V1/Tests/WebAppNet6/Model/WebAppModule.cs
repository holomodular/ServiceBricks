using ServiceBricks;
using System.Reflection;

namespace WebApp.Model
{
    public class WebAppModule : IModule
    {
        public WebAppModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(WebAppModule).Assembly
            };
        }

        public List<IModule> DependentModules { get; set; }

        public List<Assembly> AutomapperAssemblies { get; set; }

        public List<Assembly> ViewAssemblies { get; set; }
    }
}