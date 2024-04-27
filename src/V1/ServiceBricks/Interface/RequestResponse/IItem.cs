namespace ServiceBricks
{
    /// <summary>
    /// This is an item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IItem<T>
    {
        /// <summary>
        /// The typed object item.
        /// </summary>
        T Item { get; set; }
    }
}