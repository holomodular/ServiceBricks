namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all Service Brick Modules registered in the application.
    /// </summary>
    public partial class ModuleRegistry : IRegistry<Type, IModule>, IModuleRegistry
    {
        protected static ReaderWriterLock _lock = new ReaderWriterLock();

        protected static Dictionary<Type, IModule> _cache =
            new Dictionary<Type, IModule>();

        /// <summary>
        /// The singelton instance of the module registry.
        /// </summary>
        public static ModuleRegistry Instance = new ModuleRegistry();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleRegistry()
        {
            _lock = new ReaderWriterLock();
            _cache = new Dictionary<Type, IModule>();
        }

        /// <summary>
        /// Get a list of all modules.
        /// </summary>
        /// <returns></returns>
        public virtual List<IModule> GetModules()
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                return _cache.Values.ToList();
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IModule GetRegistryItem(Type key)
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (_cache.ContainsKey(key))
                    return _cache[key];
                return null;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public virtual void RegisterItem(Type key, IModule data)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (_cache.ContainsKey(key))
                    _cache[key] = data;
                else
                    _cache.Add(key, data);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        public virtual void UnRegisterItem(Type key)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (_cache.ContainsKey(key))
                    _cache.Remove(key);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<Type> GetKeys()
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                return _cache.Keys;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }
    }
}