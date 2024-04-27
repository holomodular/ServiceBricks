namespace ServiceBricks
{
    /// <summary>
    /// This provides the information used to process a business rule.
    /// </summary>
    public interface IBusinessRuleContext
    {
        object Object { get; set; }
        Dictionary<string, object> Data { get; set; }
    }
}