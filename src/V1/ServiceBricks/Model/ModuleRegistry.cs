namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all modules registered in the application.
    /// </summary>
    public partial class ModuleRegistry : IModuleRegistry
    {
        /// <summary>
        /// Lock object for cache
        /// </summary>
        public static ReaderWriterLock LockObject = new ReaderWriterLock();

        /// <summary>
        /// Cache
        /// </summary>
        public static List<IModule> List = new List<IModule>();

        /// <summary>
        /// Singleton instance of the business rule registry.
        /// </summary>
        public static ModuleRegistry Instance = new ModuleRegistry();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleRegistry()
        {
            LockObject = new ReaderWriterLock();
            List = new List<IModule>();
        }

        /// <summary>
        /// Register an item with context.
        /// </summary>
        /// <param name="key"></param>
        public virtual void Register(IModule key)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!List.Contains(key))
                    List.Add(key);
            }
            finally
            {
                LockObject.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        public virtual void UnRegister(IModule key)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (List.Contains(key))
                    List.Remove(key);
            }
            finally
            {
                LockObject.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<IModule> GetKeys()
        {
            LockObject.AcquireReaderLock(Timeout.Infinite);
            try
            {
                List<IModule> modules = new List<IModule>();
                modules.AddRange(List);
                return modules;
            }
            finally
            {
                LockObject.ReleaseReaderLock();
            }
        }
    }
}