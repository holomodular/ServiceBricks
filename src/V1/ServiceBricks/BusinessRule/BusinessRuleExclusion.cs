namespace ServiceBricks
{
    /// <summary>
    /// This allows the exclusion on business rules while processing.
    /// </summary>
    public partial class BusinessRuleExclusion : BusinessRule, IBusinessRuleExclusion
    {
        public BusinessRuleExclusion() : base()
        {
        }

        public virtual System.Collections.Generic.IList<string> ExcludedDomainRuleKeys { get; set; }

        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            return new Response();
        }

        public override Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            return Task.FromResult<IResponse>(new Response());
        }
    }
}