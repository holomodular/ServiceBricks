using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceBricks.Business;

namespace ServiceBricks
{
    /// <summary>
    /// This is the business rule service.
    /// </summary>
    public partial class BusinessRuleService : IBusinessRuleService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<BusinessRuleService> _logger;
        protected readonly IBusinessRuleRegistry _businessRuleRegistry;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="businessRuleRegistry"></param>
        public BusinessRuleService(
            IServiceProvider serviceProvider,
            IBusinessRuleRegistry businessRuleRegistry
            )
        {
            _serviceProvider = serviceProvider;
            _businessRuleRegistry = businessRuleRegistry;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logFactory"></param>
        /// <param name="businessRuleRegistry"></param>
        public BusinessRuleService(
            IServiceProvider serviceProvider,
            ILoggerFactory logFactory,
            IBusinessRuleRegistry businessRuleRegistry
            )
        {
            _logger = logFactory.CreateLogger<BusinessRuleService>();
            _serviceProvider = serviceProvider;
            _businessRuleRegistry = businessRuleRegistry;
        }

        /// <summary>
        /// Execute an event.
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public virtual IResponse ExecuteEvent(IDomainEvent domainEvent)
        {
            BusinessRuleContext context = new BusinessRuleContext(domainEvent);
            return ExecuteRules(context);
        }

        /// <summary>
        /// Execute an event.
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ExecuteEventAsync(IDomainEvent domainEvent)
        {
            BusinessRuleContext context = new BusinessRuleContext(domainEvent);
            return await ExecuteRulesAsync(context);
        }

        /// <summary>
        /// Execute a process.
        /// </summary>
        /// <param name="domainProcess"></param>
        /// <returns></returns>
        public virtual IResponse ExecuteProcess(IDomainProcess domainProcess)
        {
            BusinessRuleContext context = new BusinessRuleContext(domainProcess);
            return ExecuteRules(context);
        }

        /// <summary>
        /// Execute a process.
        /// </summary>
        /// <param name="domainProcess"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ExecuteProcessAsync(IDomainProcess domainProcess)
        {
            BusinessRuleContext context = new BusinessRuleContext(domainProcess);
            return await ExecuteRulesAsync(context);
        }

        /// <summary>
        /// Execute Domain rules against the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual IResponse ExecuteRules(IBusinessRuleContext context)
        {
            return ExecuteRules(context, null);
        }

        /// <summary>
        /// Execute Domain rules against the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ExecuteRulesAsync(IBusinessRuleContext context)
        {
            return await ExecuteRulesAsync(context, null);
        }

        /// <summary>
        /// Execute Domain rules against the context, along with additonal Domain rules.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="additionalRules"></param>
        /// <returns></returns>
        public virtual IResponse ExecuteRules(IBusinessRuleContext context, IList<IBusinessRule> additionalRules)
        {
            Response response = new Response();

            try
            {
                //input validation
                if (context == null || context.Object == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                    return response;
                }

                //Find all Domain rule types for this object type
                Type itemType = context.Object.GetType();
                IList<RegistryContext<Type>> registry = _businessRuleRegistry.GetRegistryList(itemType);
                if (registry == null || registry.Count == 0)
                {
                    if (additionalRules == null || additionalRules.Count == 0)
                        return response; //No rules found, ok
                }

                //Create each type and add to list to execute from registry
                List<IBusinessRule> rules = new List<IBusinessRule>();
                if (registry != null)
                {
                    foreach (var type in registry)
                    {
                        try
                        {
                            //Create the rule
                            IBusinessRule rule = ActivatorUtilities.CreateInstance(_serviceProvider, type.Value) as IBusinessRule;
                            if (rule != null)
                            {
                                rule.SetDefinitionData(type.DefinitionData);
                                rules.Add(rule);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (_logger != null)
                                _logger.LogError(ex, $"Creating registered rule {type.Value}");
                        }
                    }
                }

                //Add any additional rules
                if (additionalRules != null && additionalRules.Count > 0)
                    rules.AddRange(additionalRules);

                //Disable Domain rule options from context
                List<string> disabledDomainRules = new List<string>();
                foreach (var rule in rules)
                {
                    if (rule is IBusinessRuleExclusion)
                    {
                        IBusinessRuleExclusion bexclusion = rule as IBusinessRuleExclusion;
                        if (bexclusion.ExcludedDomainRuleKeys != null)
                            disabledDomainRules.AddRange(bexclusion.ExcludedDomainRuleKeys);
                    }
                }

                //Order rules by priority
                var orderedRules = rules.OrderBy(x => x.Priority);

                // Check if cancellation requested before processing rules
                if (context.CancellationToken.HasValue && context.CancellationToken.Value.IsCancellationRequested)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }

                //Process each rule
                foreach (IBusinessRule rule in orderedRules)
                {
                    //Skip disabled Domain rules by context and exlusion
                    if (disabledDomainRules.Contains(rule.Name))
                        continue;

                    //Execute rule
                    IResponse resp = rule.ExecuteRule(context);
                    response.CopyFrom(resp);
                    if (!resp.Success && rule.StopOnError)
                        break;
                }

                //Return response
                return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);

                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_BUSINESS_RULE));
                return response;
            }
        }

        /// <summary>
        /// Execute Domain rules against the context, along with additonal Domain rules.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="additionalRules"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ExecuteRulesAsync(IBusinessRuleContext context, System.Collections.Generic.IList<IBusinessRule> additionalRules)
        {
            Response response = new Response();

            try
            {
                //input validation
                if (context == null || context.Object == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                    return response;
                }

                //Find all Domain rule types for this object type
                Type itemType = context.Object.GetType();
                IList<RegistryContext<Type>> registry = _businessRuleRegistry.GetRegistryList(itemType);
                if (registry == null || registry.Count == 0)
                {
                    if (additionalRules == null || additionalRules.Count == 0)
                        return response; //No rules found, ok
                }

                //Create each type and add to list to execute from registry
                List<IBusinessRule> rules = new List<IBusinessRule>();
                if (registry != null)
                {
                    foreach (var type in registry)
                    {
                        try
                        {
                            //Create the rule
                            IBusinessRule rule = ActivatorUtilities.CreateInstance(_serviceProvider, type.Value) as IBusinessRule;
                            if (rule != null)
                            {
                                rules.Add(rule);
                                rule.SetDefinitionData(type.DefinitionData);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (_logger != null)
                                _logger.LogError(ex, $"Creating registered rule {type.Value}");
                        }
                    }
                }

                //Add any additional rules
                if (additionalRules != null && additionalRules.Count > 0)
                    rules.AddRange(additionalRules);

                //Disable Domain rule options from context
                List<string> disabledDomainRules = new List<string>();
                foreach (var rule in rules)
                {
                    if (rule is IBusinessRuleExclusion)
                    {
                        IBusinessRuleExclusion bexclusion = rule as IBusinessRuleExclusion;
                        if (bexclusion.ExcludedDomainRuleKeys != null)
                            disabledDomainRules.AddRange(bexclusion.ExcludedDomainRuleKeys);
                    }
                }

                //Order rules by priority
                var orderedRules = rules.OrderBy(x => x.Priority);

                // Check if cancellation requested before processing rules
                if (context.CancellationToken.HasValue && context.CancellationToken.Value.IsCancellationRequested)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }

                //Process each rule
                foreach (IBusinessRule rule in orderedRules)
                {
                    //Skip disabled Domain rules by context and exlusion
                    if (disabledDomainRules.Contains(rule.Name))
                        continue;

                    //Execute rule
                    IResponse resp = await rule.ExecuteRuleAsync(context);
                    response.CopyFrom(resp);
                    if (!resp.Success && rule.StopOnError)
                        break;
                }

                //Return response
                return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);

                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_BUSINESS_RULE));
                return response;
            }
        }
    }
}