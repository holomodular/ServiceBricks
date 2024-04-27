namespace ServiceBricks
{
    /// <summary>
    /// This is a response list with paging.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IResponseAggregateCountList<T> : IResponseList<T>
    {
        double? Aggregate { get; set; }
        int? Count { get; set; }
    }
}