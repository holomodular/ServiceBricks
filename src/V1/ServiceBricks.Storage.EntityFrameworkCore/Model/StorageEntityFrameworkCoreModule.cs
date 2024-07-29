using System.Reflection;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    public class StorageEntityFrameworkCoreModule : IModule
    {
        public StorageEntityFrameworkCoreModule()
        {
        }

        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}