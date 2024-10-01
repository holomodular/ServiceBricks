namespace ServiceBricks
{
    /// <summary>
    /// Service that executes business rules.
    /// </summary>
    public partial interface IBusinessRuleService
    {
        /// <summary>
        /// Execute a Domain Event
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        IResponse ExecuteEvent(IDomainEvent domainEvent);

        /// <summary>
        /// Execute a Domain Event
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        Task<IResponse> ExecuteEventAsync(IDomainEvent domainEvent);

        /// <summary>
        /// Execute a Domain Process.
        /// </summary>
        /// <param name="domainProcess"></param>
        /// <returns></returns>
        IResponse ExecuteProcess(IDomainProcess domainProcess);

        /// <summary>
        /// Execute a Domain Process.
        /// </summary>
        /// <param name="domainProcess"></param>
        /// <returns></returns>
        Task<IResponse> ExecuteProcessAsync(IDomainProcess domainProcess);

        /// <summary>
        /// Execute business rules against the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IResponse ExecuteRules(IBusinessRuleContext context);

        /// <summary>
        /// Execute business rules against the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<IResponse> ExecuteRulesAsync(IBusinessRuleContext context);

        /// <summary>
        /// Execute business rules against the context, along with additonal business rules.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="additionalRules"></param>
        /// <returns></returns>
        IResponse ExecuteRules(IBusinessRuleContext context, System.Collections.Generic.IList<IBusinessRule> additionalRules);

        /// <summary>
        /// Execute business rules against the context, along with additonal business rules.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="additionalRules"></param>
        /// <returns></returns>
        Task<IResponse> ExecuteRulesAsync(IBusinessRuleContext context, System.Collections.Generic.IList<IBusinessRule> additionalRules);
    }
}