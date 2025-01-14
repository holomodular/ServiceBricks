using System.Reflection;

namespace WebApp.Model
{
    public class WebAppModule : ServiceBricks.Module
    {
        public WebAppModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(WebAppModule).Assembly
            };
        }
    }
}