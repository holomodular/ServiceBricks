namespace ServiceBricks
{
    /// <summary>
    /// This allows processing from a database table like a queue.
    /// </summary>
    public interface IDpProcessQueue : IDpCreateDate
    {
        int RetryCount { get; set; }
        bool IsError { get; set; }
        bool IsProcessing { get; set; }
        bool IsComplete { get; set; }
        DateTimeOffset FutureProcessDate { get; set; }
        DateTimeOffset ProcessDate { get; set; }
        string ProcessResponse { get; set; }
    }
}