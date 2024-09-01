namespace ServiceBricks
{
    /// <summary>
    /// This is a registry list of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial interface IRegistryList<TKey, TValue>
    {
        /// <summary>
        /// Get a list of items.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<RegistryContext<TValue>> GetRegistryList(TKey key);

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void RegisterItem(TKey key, TValue value);

        /// <summary>
        /// Register an item with context.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domainRuleContext"></param>
        void RegisterItem(TKey key, TValue value, Dictionary<string, object> custom);

        /// <summary>
        /// Unregister all items.
        /// </summary>
        /// <param name="key"></param>
        void UnRegister(TKey key);

        /// <summary>
        /// Unregister a single item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UnRegisterItem(TKey key, TValue value);

        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        ICollection<TKey> GetKeys();
    }
}