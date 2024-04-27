using System.Reflection;

namespace ServiceBricks.Xunit
{
    public class ServiceBrickTestModule : IModule
    {
        public ServiceBrickTestModule()
        {
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}