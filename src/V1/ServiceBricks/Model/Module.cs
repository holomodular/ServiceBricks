using System.Reflection;

namespace ServiceBricks
{
    public abstract partial class Module : IModule
    {
        /// <summary>
        /// The name of the module.
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        /// <summary>
        /// Flag to denote if the module has been started.
        /// </summary>
        public virtual bool Started { get; set; }

        /// <summary>
        /// The start priority of the module.
        /// </summary>
        public virtual int StartPriority { get; set; } = BusinessRule.PRIORITY_NORMAL;

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public virtual List<IModule> DependentModules { get; set; }

        /// <summary>
        ///  The list of automapper assemblies.
        /// </summary>
        public virtual List<Assembly> AutomapperAssemblies { get; set; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public virtual List<Assembly> ViewAssemblies { get; set; }

        /// <summary>
        /// Check equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(IModule other)
        {
            if (other == null)
                return false;

            return this.FullName == other.FullName;
        }

        /// <summary>
        /// Get the hashcode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }
    }
}