namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial class MapperRegistry : IMapperRegistry
    {
        /// <summary>
        /// Lock object for cache
        /// </summary>
        public static ReaderWriterLock LockObject = new ReaderWriterLock();

        /// <summary>
        /// Cache
        /// </summary>
        public static Dictionary<Type, IList<MapperRegistryValue>> Cache =
            new Dictionary<Type, IList<MapperRegistryValue>>();

        /// <summary>
        /// Singleton instance of the business rule registry.
        /// </summary>
        public static MapperRegistry Instance = new MapperRegistry();

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<MapperRegistryValue> GetRegistryList(Type key)
        {
            LockObject.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (!Cache.ContainsKey(key))
                    return null;

                var existing = Cache[key];
                var list = new List<MapperRegistryValue>();
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
        /// Get a mapping
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Action<object, object> GetMapper(Type source, Type destination)
        {
            LockObject.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (!Cache.ContainsKey(source))
                    return null;

                var existing = Cache[source];
                var found = existing.Where(x => x.DestinationType == destination).FirstOrDefault();
                return found?.Mapper;
            }
            finally
            {
                LockObject.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Register a mapper config
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapper"></param>
        public void Register<TSource, TDestination>(Action<TSource, TDestination> mapper)
        {
            Action<object, object> wrapper = (src, dest) =>
                mapper((TSource)src, (TDestination)dest);

            Register(typeof(TSource), typeof(TDestination), wrapper);
        }

        /// <summary>
        /// Register a mapper config
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="mapper"></param>
        public virtual void Register(Type source, Type destination, Action<object, object> mapper)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (Cache.ContainsKey(source))
                {
                    var existing = Cache[source];
                    // Check duplicate
                    bool found = false;
                    for (int i = 0; i < existing.Count; i++)
                    {
                        if (existing[i].DestinationType.Equals(destination))
                        {
                            found = true;
                            existing[i].Mapper = mapper; // Overwrite existing
                            break;
                        }
                    }
                    if (!found)
                        existing.Add(new MapperRegistryValue() { DestinationType = destination, Mapper = mapper });
                    Cache[source] = existing;
                }
                else
                {
                    var newlist = new List<MapperRegistryValue>();
                    newlist.Add(new MapperRegistryValue() { DestinationType = destination, Mapper = mapper });
                    Cache.Add(source, newlist);
                }
            }
            finally
            {
                LockObject.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="source"></param>
        public virtual void UnRegister(Type source)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (Cache.ContainsKey(source))
                    Cache.Remove(source);
            }
            finally
            {
                LockObject.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Unregister a source and destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public virtual void UnRegister(Type source, Type destination)
        {
            LockObject.AcquireWriterLock(Timeout.Infinite);

            try
            {
                if (!Cache.ContainsKey(source))
                    return;

                var existing = Cache[source];
                for (int i = 0; i < existing.Count; i++)
                {
                    if (existing[i].DestinationType.Equals(destination))
                    {
                        existing.RemoveAt(i);
                        break;
                    }
                }

                if (existing.Count == 0)
                    Cache.Remove(source);
                else
                    Cache[source] = existing;
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