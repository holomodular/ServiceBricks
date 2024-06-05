using System.Collections.Concurrent;

namespace ServiceBricks
{
    /// <summary>
    /// This is a registry list of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class RegistryList<TKey, TValue> : IRegistryList<TKey, TValue>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RegistryList()
        {
        }

        /// <summary>
        /// The collection of registry entries.
        /// </summary>
        public ConcurrentDictionary<TKey, System.Collections.Generic.IList<RegistryContext<TValue>>> Cache = new ConcurrentDictionary<TKey, System.Collections.Generic.IList<RegistryContext<TValue>>>();

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual System.Collections.Generic.IList<RegistryContext<TValue>> GetRegistryList(TKey key)
        {
            System.Collections.Generic.IList<RegistryContext<TValue>> val = null;
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
            System.Collections.Generic.IList<RegistryContext<TValue>> existing;
            if (Cache.TryGetValue(key, out existing))
            {
                foreach (var item in existing)
                {
                    if (item.Equals(data))
                        return;
                }
                existing.Add(new RegistryContext<TValue>() { Value = data });
                if (!Cache.TryUpdate(key, existing, existing))
                {
                    //ERROR
                }
            }
            else
            {
                existing = new List<RegistryContext<TValue>>();
                existing.Add(new RegistryContext<TValue>() { Value = data });
                if (!Cache.TryAdd(key, existing))
                {
                    //ERROR
                }
            }
        }

        /// <summary>
        /// Register an item with context.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="domainRuleContext"></param>
        public void RegisterItem(TKey key, TValue data, Dictionary<string, object> custom)
        {
            System.Collections.Generic.IList<RegistryContext<TValue>> existing;
            if (Cache.TryGetValue(key, out existing))
            {
                existing.Add(new RegistryContext<TValue>() { Custom = custom, Value = data });
                if (!Cache.TryUpdate(key, existing, existing))
                {
                    //ERROR
                }
            }
            else
            {
                existing = new List<RegistryContext<TValue>>();
                existing.Add(new RegistryContext<TValue>() { Custom = custom, Value = data });
                if (!Cache.TryAdd(key, existing))
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
            System.Collections.Generic.IList<RegistryContext<TValue>> existing;
            Cache.TryRemove(key, out existing);
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void UnRegisterItem(TKey key, TValue val)
        {
            System.Collections.Generic.IList<RegistryContext<TValue>> existing;
            if (Cache.TryGetValue(key, out existing))
            {
                for (int i = 0; i < existing.Count; i++)
                {
                    if (existing[i].Value.Equals(val))
                    {
                        existing.RemoveAt(i);
                        break;
                    }
                }
                if (!Cache.TryUpdate(key, existing, existing))
                {
                    //ERROR
                }
            }
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