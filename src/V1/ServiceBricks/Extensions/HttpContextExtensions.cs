using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ServiceBricks
{
    /// <summary>
    /// ClaimsPrincipal extensions for the Core module.
    /// </summary>
    public static partial class HttpContextExtensions
    {
        /// <summary>
        /// Get the user's storage key.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserStorageKey(this HttpContext context)
        {
            return context?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        }
    }
}