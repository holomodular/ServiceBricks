using System.Reflection;

namespace ServiceBricks.Xunit
{
    public class ServiceBricksTestModule : IModule
    {
        public ServiceBricksTestModule()
        {
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}