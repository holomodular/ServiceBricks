namespace ServiceBricks
{
    /// <summary>
    /// This allows the exclusion on business rules while processing.
    /// </summary>
    public interface IBusinessRuleExclusion
    {
        System.Collections.Generic.IList<string> ExcludedDomainRuleKeys { get; set; }
    }
}