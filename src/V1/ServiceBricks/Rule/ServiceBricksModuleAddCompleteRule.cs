using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ServiceBricks
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class ServiceBricksModuleAddCompleteRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceBricksModuleAddCompleteRule()
        {
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddCompleteEvent<ServiceBricksModule>),
                typeof(ServiceBricksModuleAddCompleteRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddCompleteEvent<ServiceBricksModule>),
                typeof(ServiceBricksModuleAddCompleteRule));
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
            var e = context.Object as ModuleAddCompleteEvent<ServiceBricksModule>;
            if (e == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

            // Configure modelstate errors to use response object if needed
            services.Configure<ApiBehaviorOptions>(x => x.InvalidModelStateResponseFactory = context =>
            {
                ObjectResult objectResult = null;
                if (context.HttpContext != null &&
                    context.HttpContext.Request != null &&
                    context.HttpContext.Request.Path.HasValue &&
                    context.HttpContext.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var apiOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApiOptions>>().Value;

                    if (apiOptions.ReturnResponseObject)
                    {
                        Response response = new Response();
                        foreach (var key in context.ModelState.Keys)
                        {
                            foreach (var err in context.ModelState[key].Errors)
                            {
                                if (!string.IsNullOrEmpty(key))
                                    response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage, key));
                                else
                                    response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage));
                            }
                        }
                        objectResult = new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
                        return objectResult;
                    }
                }
                var vpd = new ValidationProblemDetails(context.ModelState);
                objectResult = new ObjectResult(vpd) { StatusCode = StatusCodes.Status400BadRequest };
                return objectResult;
            });

            return response;
        }
    }
}