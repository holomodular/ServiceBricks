namespace ServiceBricks
{
    /// <summary>
    /// This provides the information used to process a business rule.
    /// </summary>
    public partial interface IBusinessRuleContext
    {
        /// <summary>
        /// The object to process.
        /// </summary>
        object Object { get; set; }

        /// <summary>
        /// Extra data that can be passed to other rules while processing.
        /// </summary>
        Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// The cancellation token.
        /// </summary>
        CancellationToken? CancellationToken { get; set; }
    }
}