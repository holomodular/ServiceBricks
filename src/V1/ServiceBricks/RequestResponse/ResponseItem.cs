namespace ServiceBricks
{
    /// <summary>
    /// This is a response item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ResponseItem<T> : Response, IResponseItem<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResponseItem() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item"></param>
        public ResponseItem(T item) : base()
        {
            Item = item;
        }

        /// <summary>
        /// The response typed object item.
        /// </summary>
        public virtual T Item { get; set; }
    }
}