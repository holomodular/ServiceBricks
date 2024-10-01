namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule.
    /// </summary>
    public partial interface IBusinessRule
    {
        /// <summary>
        /// The name of the rule.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Determines if the rule should stop on error.
        /// </summary>
        bool StopOnError { get; }

        /// <summary>
        /// The priority of the rule.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Definition information for the rule
        /// </summary>
        Dictionary<string, object> DefinitionData { get; set; }

        /// <summary>
        /// Sets custom data for the rule.
        /// </summary>
        /// <param name="definitionData"></param>
        void SetDefinitionData(Dictionary<string, object> definitionData);

        /// <summary>
        /// Executes the rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IResponse ExecuteRule(IBusinessRuleContext context);

        /// <summary>
        /// Executes the rule asynchronously.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context);
    }
}