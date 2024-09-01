namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule.
    /// </summary>
    public abstract partial class BusinessRule : IBusinessRule
    {
        /// <summary>
        /// Before Low Priority of the rule
        /// </summary>
        public const int PRIORITY_BEFORE_LOW = 10000;

        /// <summary>
        /// Low Priority of the rule
        /// </summary>
        public const int PRIORITY_LOW = 20000;

        /// <summary>
        /// After Low Priority of the rule
        /// </summary>
        public const int PRIORITY_AFTER_LOW = 30000;

        /// <summary>
        /// Normal Priority of the rule
        /// </summary>
        public const int PRIORITY_NORMAL = 0;

        /// <summary>
        /// Before High Priority of the rule
        /// </summary>
        public const int PRIORITY_BEFORE_HIGH = -30000;

        /// <summary>
        /// High Priority of the rule
        /// </summary>
        public const int PRIORITY_HIGH = -20000;

        /// <summary>
        /// After High Priority of the rule
        /// </summary>
        public const int PRIORITY_AFTER_HIGH = -10000;

        /// <summary>
        /// Constructor
        /// </summary>
        public BusinessRule()
        {
            Priority = PRIORITY_NORMAL;
            StopOnError = true;
            CustomData = new Dictionary<string, object>();
        }

        /// <summary>
        /// Name of the rule
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        /// <summary>
        /// Custom data for the rule
        /// </summary>
        public virtual Dictionary<string, object> CustomData { get; set; }

        /// <summary>
        /// Stop on error
        /// </summary>
        public virtual bool StopOnError { get; set; }

        /// <summary>
        /// Priority of the rule
        /// </summary>
        public virtual int Priority { get; set; }

        /// <summary>
        /// Execute the rule
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract IResponse ExecuteRule(IBusinessRuleContext context);

        /// <summary>
        /// Execute the rule asynchronously
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            return Task.FromResult(ExecuteRule(context));
        }

        /// <summary>
        /// Set custom data
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetCustomData(Dictionary<string, object> data)
        {
            if (data != null)
                CustomData = data;
        }
    }
}