using Microsoft.AspNetCore.Http;

namespace ServiceBricks
{
    /// <summary>
    /// This service gets the current IP Address.
    /// </summary>
    public partial class IpAddressService : IIpAddressService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public IpAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the IP Address
        /// </summary>
        /// <returns></returns>
        public virtual string GetIPAddress()
        {
            return GetIPAddress(_httpContextAccessor?.HttpContext);
        }

        /// <summary>
        /// Get the IP Address
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetIPAddress(HttpContext httpContext)
        {
            return httpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}