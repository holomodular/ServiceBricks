using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;

namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial class BusinessRuleRegistry : IRegistryList<Type, Type>, IBusinessRuleRegistry
    {
        protected static ReaderWriterLock _lock = new ReaderWriterLock();

        protected static Dictionary<Type, IList<RegistryContext<Type>>> _cache =
            new Dictionary<Type, IList<RegistryContext<Type>>>();

        internal BusinessRuleRegistry()
        {
            _lock = new ReaderWriterLock();
            _cache = new Dictionary<Type, IList<RegistryContext<Type>>>();
        }

        /// <summary>
        /// Singleton instance of the business rule registry.
        /// </summary>
        public static BusinessRuleRegistry Instance = new BusinessRuleRegistry();

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<RegistryContext<Type>> GetRegistryList(Type key)
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (!_cache.ContainsKey(key))
                    return null;

                var existing = _cache[key];
                var list = new List<RegistryContext<Type>>();
                foreach (var item in existing)
                    list.Add(item);
                return list;
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
        public virtual void RegisterItem(Type key, Type data)
        {
            RegisterItem(key, data, null);
        }

        /// <summary>
        /// Register an item with context.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="domainRuleContext"></param>
        public virtual void RegisterItem(Type key, Type data, Dictionary<string, object> custom)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (_cache.ContainsKey(key))
                {
                    // Check duplicates
                    var existing = _cache[key];
                    foreach (var item in existing)
                    {
                        if (item.Value.Equals(data))
                            return;
                    }
                    existing.Add(new RegistryContext<Type>() { CustomData = custom, Value = data });
                    _cache[key] = existing;
                }
                else
                {
                    var newlist = new List<RegistryContext<Type>>();
                    newlist.Add(new RegistryContext<Type>() { CustomData = custom, Value = data });
                    _cache.Add(key, newlist);
                }
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
        public virtual void UnRegister(Type key)
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
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void UnRegisterItem(Type key, Type val)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!_cache.ContainsKey(key))
                    return;

                var existing = _cache[key];
                for (int i = 0; i < existing.Count; i++)
                {
                    if (existing[i].Value.Equals(val))
                    {
                        existing.RemoveAt(i);
                        break;
                    }
                }

                if (existing.Count == 0)
                    _cache.Remove(key);
                else
                    _cache[key] = existing;
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<Type> GetKeys()
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                var keys = _cache.Keys;
                var list = new List<Type>();
                foreach (var item in keys)
                    list.Add(item);
                return list;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }
    }
}