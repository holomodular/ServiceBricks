using System.Reflection;

namespace ServiceBricks
{
    /// <summary>
    /// The module definition for the Service Bricks module.
    /// </summary>
    public partial class ServiceBricksModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceBricksModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ServiceBricksModule).Assembly
            };
        }

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of automapper assemblies.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}