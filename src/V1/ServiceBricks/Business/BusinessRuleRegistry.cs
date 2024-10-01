using ServiceBricks.Business;

namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial class BusinessRuleRegistry : IRegistryList<Type, Type>, IBusinessRuleRegistry
    {
        /// <summary>
        /// Lock object for cache
        /// </summary>
        public static ReaderWriterLock LockObject = new ReaderWriterLock();

        /// <summary>
        /// Cache
        /// </summary>
        public static Dictionary<Type, IList<RegistryContext<Type>>> Cache =
            new Dictionary<Type, IList<RegistryContext<Type>>>();

        /// <summary>
        /// Singleton instance of the business rule registry.
        /// </summary>
        public static BusinessRuleRegistry Instance = new BusinessRuleRegistry();

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusinessRuleRegistry()
        {
            LockObject = new ReaderWriterLock();
            Cache = new Dictionary<Type, IList<RegistryContext<Type>>>();
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<RegistryContext<Type>> GetRegistryList(Type key)
        {
            LockObject.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (!Cache.ContainsKey(key))
                    return null;

                var existing = Cache[key];
                var list = new List<RegistryContext<Type>>();
                foreach (var item in existing)
                    list.Add(item);
                return list;
            }
            finally
            {
                LockObject.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public virtual void Register(Type key, Type data)
        {
            Register(key, data, null);
        }

        /// <summary>
        /// Register an item with context.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="definitionData"></param>
        public virtual void Register(Type key, Type data, Dictionary<string, object> definitionData)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (Cache.ContainsKey(key))
                {
                    // Check duplicates
                    var existing = Cache[key];
                    foreach (var item in existing)
                    {
                        if (item.Value.Equals(data))
                        {
                            if (IsDuplicate(item.DefinitionData, definitionData))
                                return;
                        }
                    }

                    existing.Add(new RegistryContext<Type>() { DefinitionData = definitionData, Value = data });
                    Cache[key] = existing;
                }
                else
                {
                    var newlist = new List<RegistryContext<Type>>();
                    newlist.Add(new RegistryContext<Type>() { DefinitionData = definitionData, Value = data });
                    Cache.Add(key, newlist);
                }
            }
            finally
            {
                LockObject.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Determine if the dictionaries are the same
        /// </summary>
        /// <param name="dic1"></param>
        /// <param name="dic2"></param>
        /// <returns></returns>
        protected virtual bool IsDuplicate(Dictionary<string, object> dic1, Dictionary<string, object> dic2)
        {
            if (dic1 == null && dic2 == null)
                return true;
            if (dic1 == null && dic2 != null)
                return false;
            if (dic1 != null && dic2 == null)
                return false;
            if (dic1.Keys.Count != dic2.Keys.Count)
                return false;

            foreach (var itemdic1Key in dic2.Keys)
            {
                if (!dic1.ContainsKey(itemdic1Key))
                    return false;

                var dic1val = dic1[itemdic1Key];
                var dic2val = dic2[itemdic1Key];

                if (dic1val == null && dic2val == null)
                    continue;

                if (dic1val != null && dic2val == null)
                    return false;
                if (dic1val == null && dic2val != null)
                    return false;

                if (dic1val is IList<string> && dic2val is IList<string>)
                {
                    if (!IsDuplicate(dic1val as IList<string>, dic2val as IList<string>))
                        return false;
                }
                else
                {
                    if (!dic2val.Equals(dic1val))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determine if lists are the same
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        protected virtual bool IsDuplicate(IList<string> list1, IList<string> list2)
        {
            if (list1 == null && list2 == null)
                return true;
            if (list1 == null && list2 != null)
                return false;
            if (list1 != null && list2 == null)
                return false;
            if (list1.Count != list2.Count)
                return false;
            foreach (string list1key in list1)
            {
                if (!list2.Contains(list1key))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        public virtual void UnRegister(Type key)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (Cache.ContainsKey(key))
                    Cache.Remove(key);
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
        /// <param name="val"></param>
        public virtual void UnRegister(Type key, Type val)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!Cache.ContainsKey(key))
                    return;

                var existing = Cache[key];
                for (int i = 0; i < existing.Count; i++)
                {
                    if (existing[i].Value.Equals(val))
                    {
                        existing.RemoveAt(i);
                        i--;
                    }
                }

                if (existing.Count == 0)
                    Cache.Remove(key);
                else
                    Cache[key] = existing;
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
        /// <param name="val"></param>
        /// <param name="custom"></param>
        public virtual void UnRegister(Type key, Type val, Dictionary<string, object> custom)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!Cache.ContainsKey(key))
                    return;

                var existing = Cache[key];
                for (int i = 0; i < existing.Count; i++)
                {
                    if (existing[i].Value.Equals(val))
                    {
                        if (IsDuplicate(existing[i].DefinitionData, custom))
                        {
                            existing.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (existing.Count == 0)
                    Cache.Remove(key);
                else
                    Cache[key] = existing;
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
        public virtual ICollection<Type> GetKeys()
        {
            LockObject.AcquireReaderLock(Timeout.Infinite);
            try
            {
                var keys = Cache.Keys;
                var list = new List<Type>();
                foreach (var item in keys)
                    list.Add(item);
                return list;
            }
            finally
            {
                LockObject.ReleaseReaderLock();
            }
        }
    }
}