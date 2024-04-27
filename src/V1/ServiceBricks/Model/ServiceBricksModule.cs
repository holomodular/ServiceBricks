using System.Reflection;

namespace ServiceBricks
{
    public class ServiceBricksModule : IModule
    {
        public ServiceBricksModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ServiceBricksModule).Assembly
            };
        }

        public List<IModule> DependentModules { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<Assembly> AutomapperAssemblies { get; }
    }
}