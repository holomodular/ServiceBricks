using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ServiceBricks
{
    /// <summary>
    /// ModelState extensions for the ServiceBricks module.
    /// </summary>
    public static partial class ModelStateExtensions
    {
        /// <summary>
        /// Copy messages from a response object to this instance.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="response"></param>
        public static void CopyFromResponse(this ModelStateDictionary modelState, IResponse response)
        {
            if (modelState == null || response == null || response.Messages == null)
                return;

            foreach (var item in response.Messages)
            {
                if (item.Severity == ResponseSeverity.Error)
                {
                    string field = string.Empty;
                    if (item.Fields != null && item.Fields.Count > 0)
                        field = item.Fields[0];
                    modelState.AddModelError(field, item.Message);
                }
            }
        }

        /// <summary>
        /// Copy messages from this to the response object.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="response"></param>
        public static void CopyToResponse(this ModelStateDictionary modelState, IResponse response)
        {
            if (modelState == null || response == null || response.Messages == null)
                return;

            var list = modelState.ToList();
            foreach (var item in list)
            {
                if (item.Value.Errors != null)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        response.AddMessage(ResponseMessage.CreateError(error.ErrorMessage, item.Key));
                    }
                }
            }
        }
    }
}