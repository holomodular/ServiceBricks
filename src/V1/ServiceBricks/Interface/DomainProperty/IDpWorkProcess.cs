namespace ServiceBricks
{
    /// <summary>
    /// This allows processing from a database table like a queue.
    /// </summary>
    public partial interface IDpWorkProcess : IDpCreateDate, IDpUpdateDate
    {
        /// <summary>
        /// The retry count.
        /// </summary>
        int RetryCount { get; set; }

        /// <summary>
        /// Determines if the process is an error.
        /// </summary>
        bool IsError { get; set; }

        /// <summary>
        /// Determines if the process is processing, currently running.
        /// </summary>
        bool IsProcessing { get; set; }

        /// <summary>
        /// Determines if the process is complete.
        /// </summary>
        bool IsComplete { get; set; }

        /// <summary>
        /// The future date the process should run.
        /// </summary>
        DateTimeOffset FutureProcessDate { get; set; }

        /// <summary>
        /// The date the process ran.
        /// </summary>
        DateTimeOffset ProcessDate { get; set; }

        /// <summary>
        /// The process response (json serialized)
        /// </summary>
        string ProcessResponse { get; set; }
    }
}