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
        /// The custom data.
        /// </summary>
        Dictionary<string, object> Data { get; set; }
    }
}