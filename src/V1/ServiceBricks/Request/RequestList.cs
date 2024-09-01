namespace ServiceBricks
{
    /// <summary>
    /// This is a request list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class RequestList<T> : Request, IRequestList<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestList()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public RequestList(List<T> items)
        {
            List = items;
        }

        /// <summary>
        /// The collection of request typed object items.
        /// </summary>
        public virtual List<T> List { get; set; }
    }
}