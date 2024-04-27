namespace ServiceBricks
{
    /// <summary>
    /// This is a response list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ResponseList<T> : Response, IResponseList<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResponseList()
        {
            List = new List<T>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public ResponseList(List<T> items)
        {
            List = items;
        }

        /// <summary>
        /// The collection of response typed object items.
        /// </summary>
        public virtual List<T> List { get; set; }
    }
}