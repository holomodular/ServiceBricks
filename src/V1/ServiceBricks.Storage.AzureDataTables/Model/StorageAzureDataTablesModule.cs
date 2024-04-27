using System.Reflection;

namespace ServiceBricks.Storage.AzureDataTables
{
    public class StorageAzureDataTablesModule : IModule
    {
        public StorageAzureDataTablesModule()
        {
            AdminHtml = string.Empty;
            Name = "Storage AzureDataTables Brick";
            Description = @"The Storage AzureDataTables Brick implements the Azure Data Tables provider.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}