namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule.
    /// </summary>
    public abstract partial class BusinessRule : IBusinessRule
    {
        public const int PRIORITY_LOW = 1000;
        public const int PRIORITY_NORMAL = 0;
        public const int PRIORITY_HIGH = -1000;

        public BusinessRule()
        {
            Priority = PRIORITY_NORMAL;
            StopOnError = true;
            CustomData = new Dictionary<string, object>();
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        public virtual Dictionary<string, object> CustomData { get; set; }

        public virtual bool StopOnError { get; set; }

        public virtual int Priority { get; set; }

        public abstract IResponse ExecuteRule(IBusinessRuleContext context);

        public virtual Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            return Task.FromResult(ExecuteRule(context));
        }

        public void SetCustomData(Dictionary<string, object> data)
        {
            if (data != null)
                CustomData = data;
        }
    }
}