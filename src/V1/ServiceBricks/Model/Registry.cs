using System.Collections.Concurrent;

namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class Registry<TKey, TValue> : IRegistry<TKey, TValue>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Registry()
        {
        }

        /// <summary>
        /// The collection of registry entries.
        /// </summary>
        protected ConcurrentDictionary<TKey, TValue> Cache = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TValue GetRegistryItem(TKey key)
        {
            TValue val = default(TValue);
            Cache.TryGetValue(key, out val);
            return val;
        }

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public virtual void RegisterItem(TKey key, TValue data)
        {
            TValue existing;
            if (Cache.TryGetValue(key, out existing))
            {
                if (!Cache.TryUpdate(key, existing, data))
                {
                    //ERROR
                }
            }
            else
            {
                if (!Cache.TryAdd(key, data))
                {
                    //ERROR
                }
            }
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        public virtual void UnRegisterItem(TKey key)
        {
            TValue existing;
            Cache.TryRemove(key, out existing);
        }

        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<TKey> GetKeys()
        {
            return Cache.Keys;
        }
    }
}