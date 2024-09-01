using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// A middleware to handle exceptions.
    /// </summary>
    public partial class TrapExceptionResponseMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly ILogger<TrapExceptionResponseMiddleware> _logger;
        protected readonly IMapper _mapper;
        protected readonly ApiOptions _apiOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="mapper"></param>
        /// <param name="apiOptions"></param>
        public TrapExceptionResponseMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IOptions<ApiOptions> apiOptions)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TrapExceptionResponseMiddleware>();
            _mapper = mapper;
            _apiOptions = apiOptions.Value;
        }

        /// <summary>
        /// Runtime method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public virtual async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handle exceptions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log all exceptions
            _logger.LogError(exception, exception.Message);

            // Handle API requests
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
            {
                // Handle API requests using response or problem details
                if (_apiOptions.ReturnResponseObject)
                {
                    context.Response.ContentType = @"application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    Response response = new Response();
                    if (_apiOptions.ExposeSystemErrors)
                        response.AddMessage(ResponseMessage.CreateError(exception));
                    else
                        response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));

                    // Write response object
                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(response));
                    return;
                }

                // Set content type and status code
                context.Response.ContentType = @"application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Create problem details
                ProblemDetails details;
                if (_apiOptions.ExposeSystemErrors)
                    details = _mapper.Map<ProblemDetails>(exception);
                else
                {
                    details = new ProblemDetails()
                    {
                        Type = "https://httpstatuses.com/" + context.Response.StatusCode,
                        Title = "System Error",
                        Status = context.Response.StatusCode,
                        Detail = "General Server Error",
                    };
                }

                // Write problem details
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(details));
            }
        }
    }
}