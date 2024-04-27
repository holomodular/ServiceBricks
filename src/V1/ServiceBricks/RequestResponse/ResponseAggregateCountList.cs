namespace ServiceBricks
{
    /// <summary>
    /// This is a response list with paging.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ResponseAggregateCountList<T> : ResponseList<T>, IResponseAggregateCountList<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResponseAggregateCountList() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public ResponseAggregateCountList(List<T> items) : base(items)
        {
        }

        /// <summary>
        /// The total count of records
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// The aggregate value.
        /// </summary>
        public double? Aggregate { get; set; }
    }
}