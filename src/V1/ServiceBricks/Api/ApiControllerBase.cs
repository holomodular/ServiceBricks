using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace ServiceBricks
{
    public partial class ApiControllerBase : ControllerBase
    {
        protected readonly ApiOptions _apiOptions;

        public ApiControllerBase(IOptions<ApiOptions> apiOptions) : base()
        {
            _apiOptions = apiOptions.Value ?? new ApiOptions();
        }

        /// <summary>
        /// Determine response format
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected virtual bool UseModernResponse()
        {
            // Canonical query param: format=modern|classic
            if (Request != null)
            {
                if (Request.Query.TryGetValue("format", out var fmt))
                {
                    var v = fmt.ToString();
                    if (string.Equals(v, "modern", StringComparison.OrdinalIgnoreCase)) return true;
                    if (string.Equals(v, "classic", StringComparison.OrdinalIgnoreCase)) return false;
                }

                // Accept header override
                var accept = Request.Headers.Accept.ToString();
                if (!string.IsNullOrWhiteSpace(accept))
                {
                    if (accept.Contains("application/vnd.servicebricks.modern+json", StringComparison.OrdinalIgnoreCase)) return true;
                    if (accept.Contains("application/vnd.servicebricks.classic+json", StringComparison.OrdinalIgnoreCase)) return false;
                }
            }

            // Default
            return _apiOptions.ReturnResponseObject;
        }

        /// <summary>
        /// Get the error response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [NonAction]
        protected virtual ActionResult GetErrorResponse(IResponse response)
        {
            // Create filtered list of messages (Don't expose sensitive system errors)
            List<IResponseMessage> messages = response.Messages.ToList();
            if (!_apiOptions.ExposeSystemErrors)
                messages = messages.Where(x => x.Severity != ResponseSeverity.ErrorSystemSensitive).ToList();
            if (messages.Count == 0)
                messages.Add(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));

            // Create new error response
            var respError = new Response() { Error = true };
            foreach (var item in messages)
                respError.AddMessage(item);

            // Return response
            if (UseModernResponse())
                return BadRequest(respError);
            else
            {
                var details = new ProblemDetails()
                {
                    Title = LocalizationResource.ERROR_SYSTEM,
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = respError.GetMessage(Environment.NewLine)
                };
                return BadRequest(details);
            }
        }
    }
}