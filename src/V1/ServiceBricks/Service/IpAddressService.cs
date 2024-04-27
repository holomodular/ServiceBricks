using Microsoft.AspNetCore.Http;

namespace ServiceBricks
{
    /// <summary>
    /// This service gets the current IP Address.
    /// </summary>
    public class IpAddressService : IIpAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetIPAddress()
        {
            return GetIPAddress(_httpContextAccessor?.HttpContext);
        }

        public static string GetIPAddress(HttpContext httpContext)
        {
            var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress) && ipAddress == "::1")
                ipAddress = "127.0.0.1";
            return ipAddress;
        }
    }
}