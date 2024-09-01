namespace ServiceBricks
{
    /// <summary>
    /// This is a list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IListOfItems<T>
    {
        /// <summary>
        /// The collection of typed object items.
        /// </summary>
        List<T> List { get; set; }
    }
}