using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace ServiceBricks
{
    public partial class BaseController : ControllerBase
    {
        protected readonly ApiOptions _apiOptions;

        public BaseController(IOptions<ApiOptions> apiOptions) : base()
        {
            _apiOptions = apiOptions.Value ?? new ApiOptions();
        }

        /// <summary>
        /// Get the error response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ActionResult GetErrorResponse(IResponse response)
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
            if (_apiOptions.ReturnResponseObject)
                return BadRequest(respError);
            else
            {
                string detail = string.Join(Environment.NewLine, messages.Select(x => x.Message));
                var details = new ProblemDetails()
                {
                    Title = LocalizationResource.ERROR_SYSTEM,
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = detail
                };
                return BadRequest(details);
            }
        }
    }
}