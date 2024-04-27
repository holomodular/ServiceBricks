namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all Service Brick Modules registered in the application.
    /// </summary>
    public partial class ModuleRegistry : Registry<Type, IModule>, IModuleRegistry
    {
        public static ModuleRegistry Instance = new ModuleRegistry();

        public virtual List<IModule> GetModules()
        {
            return this.Cache.Values.ToList();
        }
    }
}