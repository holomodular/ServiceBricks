using System.Reflection;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    public class StorageEntityFrameworkCoreModule : IModule
    {
        public StorageEntityFrameworkCoreModule()
        {
            AdminHtml = string.Empty;
            Name = "Storage EntityFrameworkCore Brick";
            Description = @"The Storage EntityFrameworkCore Brick implements the Entity Framework Core provider.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}