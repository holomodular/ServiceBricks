using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ServiceBricks
{
    /// <summary>
    /// ClaimsPrincipal extensions for the Core module.
    /// </summary>
    public static partial class HttpContextAccessorExtensions
    {
        /// <summary>
        /// Get the user's storage key.
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <returns></returns>
        public static string GetUserStorageKey(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
        }
    }
}