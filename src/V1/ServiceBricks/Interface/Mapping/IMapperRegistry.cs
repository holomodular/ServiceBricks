namespace ServiceBricks
{
    /// <summary>
    /// This is a registry list of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial interface IMapperRegistry
    {
        /// <summary>
        /// Register a mapper config
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="mapper"></param>
        void Register(Type source, Type destination, Action<object, object> mapper);

        /// <summary>
        /// Register a mapper config
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapper"></param>
        void Register<TSource, TDestination>(Action<TSource, TDestination> mapper);

        /// <summary>
        /// Get the registry list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<MapperRegistryValue> GetRegistryList(Type key);

        /// <summary>
        /// Get the mapper
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        Action<object, object> GetMapper(Type source, Type destination);

        /// <summary>
        /// Unregister all matching items.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UnRegister(Type source, Type destination);
    }
}