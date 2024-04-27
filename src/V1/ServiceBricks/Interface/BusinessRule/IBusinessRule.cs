namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule.
    /// </summary>
    public partial interface IBusinessRule
    {
        string Name { get; }

        bool StopOnError { get; }

        int Priority { get; }

        void SetCustomData(Dictionary<string, object> data);

        IResponse ExecuteRule(IBusinessRuleContext context);

        Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context);
    }
}