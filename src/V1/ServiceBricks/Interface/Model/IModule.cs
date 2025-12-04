using System.Reflection;

namespace ServiceBricks
{
    /// <summary>
    /// THe module definition.
    /// </summary>
    public partial interface IModule : IEquatable<IModule>

    {
        /// <summary>
        /// The fullname of the module.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Flag to denote if the module has been started.
        /// </summary>
        public bool Started { get; set; }

        /// <summary>
        /// The start priority of the module.
        /// </summary>
        public int StartPriority { get; }

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }

        /// <summary>
        /// The list of DTOs available.
        /// </summary>
        public List<Type> DataTransferObjects { get; }
    }
}