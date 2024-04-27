namespace ServiceBricks
{
    /// <summary>
    /// This is a request item.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public partial class RequestItem<TObject> : Request, IRequestItem<TObject>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestItem()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item"></param>
        public RequestItem(TObject item)
        {
            Item = item;
        }

        /// <summary>
        /// The typed object item.
        /// </summary>
        public virtual TObject Item { get; set; }
    }
}