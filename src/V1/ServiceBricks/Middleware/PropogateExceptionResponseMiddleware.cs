using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks
{
    /// <summary>
    /// A middleware to handle exceptions.
    /// </summary>
    public partial class PropogateExceptionResponseMiddleware : TrapExceptionResponseMiddleware
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="mapper"></param>
        /// <param name="apiOptions"></param>
        public PropogateExceptionResponseMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IOptions<ApiOptions> apiOptions) : base(next, loggerFactory, mapper, apiOptions)
        {
        }

        /// <summary>
        /// Runtime method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);

                // Add the throw here so that the error is propogated down the stack
                throw;
            }
        }
    }
}