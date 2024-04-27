namespace ServiceBricks
{
    /// <summary>
    /// This is a response.
    /// </summary>
    public partial interface IResponseCount : IResponse
    {
        int? Count { get; set; }
    }
}