namespace ServiceBricks
{
    /// <summary>
    /// This provides the information used to process a business rule.
    /// </summary>
    public partial class BusinessRuleContext : IBusinessRuleContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BusinessRuleContext()
        {
            ExtraData = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public BusinessRuleContext(object obj) : this()
        {
            Object = obj;
        }

        /// <summary>
        /// The object to process.
        /// </summary>
        public virtual object Object { get; set; }

        /// <summary>
        /// Extra data that can be passed to other rules while processing.
        /// </summary>
        public virtual Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// The cancellation token.
        /// </summary>
        public virtual CancellationToken? CancellationToken { get; set; }
    }
}