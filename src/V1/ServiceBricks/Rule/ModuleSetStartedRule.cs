namespace ServiceBricks
{
    /// <summary>
    /// This rule sets a module as started
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public sealed class ModuleSetStartedRule<TModule> : BusinessRule
        where TModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleSetStartedRule()
        {
            Priority = PRIORITY_AFTER_LOW;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<TModule>),
                typeof(ModuleSetStartedRule<TModule>));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<TModule>),
                typeof(ModuleSetStartedRule<TModule>));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleStartEvent<TModule>;
            if (e == null || e.DomainObject == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            e.DomainObject.Started = true;

            return response;
        }
    }
}