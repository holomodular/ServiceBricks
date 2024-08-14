namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all Service Brick Modules registered in the application.
    /// </summary>
    public partial class ModuleRegistry : Registry<Type, IModule>, IModuleRegistry
    {
        /// <summary>
        /// The singelton instance of the module registry.
        /// </summary>
        public static ModuleRegistry Instance = new ModuleRegistry();

        /// <summary>
        /// Get a list of all modules.
        /// </summary>
        /// <returns></returns>
        public virtual List<IModule> GetModules()
        {
            return this.Cache.Values.ToList();
        }
    }
}