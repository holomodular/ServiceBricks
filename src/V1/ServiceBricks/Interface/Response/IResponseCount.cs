namespace ServiceBricks
{
    /// <summary>
    /// This is a response.
    /// </summary>
    public partial interface IResponseCount : IResponse
    {
        /// <summary>
        /// The count of items.
        /// </summary>
        int? Count { get; set; }
    }
}