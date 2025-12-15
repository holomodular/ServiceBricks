using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace WebApp.Model
{
    public class SwaggerApplySecurityOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType == null)
                return;

            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attribute => attribute.Policy!)
                .Distinct()
                .ToList();
            var parentRequiredScopes = context.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attribute => attribute.Policy!)
                .Distinct()
                .ToList();
            foreach (var parentScope in parentRequiredScopes)
            {
                if (!requiredScopes.Contains(parentScope))
                    requiredScopes.Add(parentScope);
            }
            if (requiredScopes.Count == 0)
                return;

            operation.Responses ??= new OpenApiResponses();
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            var unauthorizedStatusKey = ((int)HttpStatusCode.Unauthorized).ToString();
            if (!operation.Responses.ContainsKey(unauthorizedStatusKey))
            {
                operation.Responses.Add(
                    unauthorizedStatusKey,
                    new OpenApiResponse { Description = "Unauthorized" }
                );
            }
            var scheme = new OpenApiSecuritySchemeReference("bearer", context.Document);
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = requiredScopes
            });
        }
    }
}
