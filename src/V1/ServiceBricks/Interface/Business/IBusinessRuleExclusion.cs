namespace ServiceBricks
{
    /// <summary>
    /// This allows the exclusion on business rules while processing.
    /// </summary>
    public partial interface IBusinessRuleExclusion : IBusinessRule
    {
        /// <summary>
        /// The excluded business rule keys.
        /// </summary>
        System.Collections.Generic.IList<string> ExcludedDomainRuleKeys { get; set; }
    }
}