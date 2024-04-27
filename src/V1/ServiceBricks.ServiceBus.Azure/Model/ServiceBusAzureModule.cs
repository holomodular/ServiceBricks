using System.Reflection;

namespace ServiceBricks.ServiceBus.Azure
{
    public class ServiceBusAzureModule : IModule
    {
        public ServiceBusAzureModule()
        {
            AdminHtml = string.Empty;
            Name = "Service Bus Azure Brick";
            Description = @"The Service Bus Azure Brick implements the Azure provider.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}