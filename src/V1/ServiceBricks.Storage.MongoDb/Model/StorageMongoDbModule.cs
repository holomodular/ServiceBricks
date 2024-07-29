using System.Reflection;

namespace ServiceBricks.Storage.MongoDb
{
    public class StorageMongoDbModule : IModule
    {
        public StorageMongoDbModule()
        {
        }

        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}