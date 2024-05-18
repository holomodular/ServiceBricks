using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceBricks;
using System.Net;

namespace ServiceBricks
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly ApiOptions _apiOptions;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper,
            IOptions<ApiOptions> apiOptions)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _apiOptions = apiOptions.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log all exceptions
            _logger.LogError(exception, exception.Message);

            // Handle API requests using problem details
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_apiOptions.ReturnResponseObject)
                {
                    context.Response.ContentType = @"application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    Response response = new Response();
                    if (_webHostEnvironment.IsDevelopment() && _apiOptions.ExposeSystemErrors)
                        response.AddMessage(ResponseMessage.CreateError(exception));
                    else
                        response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));

                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(response));
                    return;
                }

                context.Response.ContentType = @"application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails details;
                if (_webHostEnvironment.IsDevelopment() && _apiOptions.ExposeSystemErrors)
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
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(details));
            }
            else
            {
                // Handle View requests using Error page
                context.Response.Redirect("/Error");
            }
        }
    }
}