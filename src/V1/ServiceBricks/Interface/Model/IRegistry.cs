namespace ServiceBricks
{
    /// <summary>
    /// This a registry of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial interface IRegistry<TKey, TValue>
    {
        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue GetRegistryItem(TKey key);

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void RegisterItem(TKey key, TValue value);

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        void UnRegisterItem(TKey key);

        /// <summary>
        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        ICollection<TKey> GetKeys();
    }
}