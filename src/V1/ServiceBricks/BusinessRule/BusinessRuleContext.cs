namespace ServiceBricks
{
    /// <summary>
    /// This provides the information used to process a business rule.
    /// </summary>
    public class BusinessRuleContext : IBusinessRuleContext
    {
        public BusinessRuleContext()
        {
            Data = new Dictionary<string, object>();
        }

        public BusinessRuleContext(object obj)
        {
            Data = new Dictionary<string, object>();
            Object = obj;
        }

        public object Object { get; set; }

        public Dictionary<string, object> Data { get; set; }
    }
}