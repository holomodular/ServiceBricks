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
            Data = new Dictionary<string, object>();
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
        /// The data to process.
        /// </summary>
        public virtual Dictionary<string, object> Data { get; set; }
    }
}