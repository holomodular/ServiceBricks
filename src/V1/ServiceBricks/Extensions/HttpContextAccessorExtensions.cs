using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ServiceBricks
{
    /// <summary>
    /// ClaimsPrincipal extensions for the Core module.
    /// </summary>
    public static class HttpContextAccessorExtensions
    {
        public static string GetUserStorageKey(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        }
    }
}