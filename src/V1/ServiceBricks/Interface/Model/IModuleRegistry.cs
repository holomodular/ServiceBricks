namespace ServiceBricks
{
    /// <summary>
    /// Registry of modules.
    /// </summary>
    public partial interface IModuleRegistry : IRegistry<Type, IModule>
    {
        /// <summary>
        /// Get the list of modules.
        /// </summary>
        /// <returns></returns>
        List<IModule> GetModules();
    }
}