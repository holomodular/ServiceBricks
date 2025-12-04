namespace ServiceBricks
{
    /// <summary>
    /// This is a registry list of items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial interface IBusinessRuleRegistryList<TKey, TValue> : IRegistry<TKey, TValue>
    {
        /// <summary>
        /// Get a list of items.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<BusinessRuleRegistryValue<TValue>> GetRegistryList(TKey key);

        /// <summary>
        /// Register an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="definitionData"></param>
        void Register(TKey key, TValue value, Dictionary<string, object> definitionData);

        /// <summary>
        /// Unregister all matching items.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UnRegister(TKey key, TValue value);

        /// <summary>
        /// Unregister all matching items
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="definitionData"></param>
        void UnRegister(TKey key, TValue value, Dictionary<string, object> definitionData);
    }
}