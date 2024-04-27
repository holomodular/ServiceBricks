using System.Reflection;

namespace ServiceBricks.Storage.MongoDb
{
    public class StorageMongoDbModule : IModule
    {
        public StorageMongoDbModule()
        {
            AdminHtml = string.Empty;
            Name = "Storage MongoDB Brick";
            Description = @"The Storage MongoDB Brick implements the MongoDB provider.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}