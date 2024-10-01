namespace ServiceBricks
{
    /// <summary>
    /// This a registry of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public partial interface IRegistry<TKey>
    {
        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        void Register(TKey key);

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        void UnRegister(TKey key);

        /// <summary>
        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        ICollection<TKey> GetKeys();
    }

    /// <summary>
    /// This a registry of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial interface IRegistry<TKey, TValue>
    {
        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Register(TKey key, TValue value);

        /// <summary>
        /// Unregister an item.
        /// </summary>
        /// <param name="key"></param>
        void UnRegister(TKey key);

        /// <summary>
        /// Get the list of managed keys.
        /// </summary>
        /// <returns></returns>
        ICollection<TKey> GetKeys();
    }
}