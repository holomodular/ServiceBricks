namespace ServiceBricks
{
    /// <summary>
    /// This allows the exclusion on business rules while processing.
    /// </summary>
    public partial class BusinessRuleExclusion : BusinessRule, IBusinessRuleExclusion
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BusinessRuleExclusion() : base()
        {
        }

        /// <summary>
        /// The list of excluded domain rule keys.
        /// </summary>
        public virtual System.Collections.Generic.IList<string> ExcludedDomainRuleKeys { get; set; }

        /// <summary>
        /// Execute the rule
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            return new Response();
        }

        /// <summary>
        /// Execute the rule asynchronously
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            return Task.FromResult<IResponse>(new Response());
        }
    }
}