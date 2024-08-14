using System.Reflection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// The module definition for the ServiceBusAzure module.
    /// </summary>
    public partial class ServiceBusAzureModule : IModule
    {
        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of assemblies that contain automapper profiles.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of assemblies that contain views.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}