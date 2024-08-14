namespace ServiceBricks
{
    /// <summary>
    /// This is a response list with paging.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IResponseAggregateCountList<T> : IResponseList<T>
    {
        /// <summary>
        /// Aggregate value.
        /// </summary>
        double? Aggregate { get; set; }

        /// <summary>
        /// The count of items.
        /// </summary>
        int? Count { get; set; }
    }
}