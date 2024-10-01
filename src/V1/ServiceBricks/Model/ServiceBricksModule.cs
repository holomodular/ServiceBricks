using System.Reflection;

namespace ServiceBricks
{
    /// <summary>
    /// The module definition for the Service Bricks module.
    /// </summary>
    public partial class ServiceBricksModule : ServiceBricks.Module
    {
        /// <summary>
        /// Static instance
        /// </summary>
        public static ServiceBricksModule Instance = new ServiceBricksModule();

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
    }
}