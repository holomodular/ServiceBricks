using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

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

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Default result
                    ObjectResult result;
                    
                    var path = context.HttpContext?.Request?.Path;                    
                    if (path.HasValue && path.Value.HasValue && 
                        path.Value.Value.StartsWith("/api/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var apiOptions = context.HttpContext.RequestServices
                            .GetRequiredService<IOptions<ApiOptions>>().Value;
                        
                        if (apiOptions.ReturnResponseObject)
                        {
                            var response = new Response();
                            context.ModelState.CopyToResponse(response);
                            result = new ObjectResult(response)
                            {
                                StatusCode = StatusCodes.Status400BadRequest
                            };
                            return result;
                        }
                    }

                    // Fallback to default ValidationProblemDetails
                    var problemDetails = new ValidationProblemDetails(context.ModelState);
                    result = new ObjectResult(problemDetails)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                    return result;
                };
            });


            return response;
        }
    }
}