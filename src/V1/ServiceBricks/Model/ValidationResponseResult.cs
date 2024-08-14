using Microsoft.AspNetCore.Mvc;

namespace ServiceBricks
{
    /// <summary>
    /// A result for validation responses.
    /// </summary>
    public partial class ValidationResponseResult : IActionResult
    {
        /// <summary>
        /// Execute the result.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            var keys = context.ModelState.Keys;
            var dic = context.ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            Response response = new Response();
            foreach (var key in context.ModelState.Keys)
            {
                foreach (var err in context.ModelState[key].Errors)
                {
                    if (!string.IsNullOrEmpty(key))
                        response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage, key));
                    else
                        response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage));
                }
            }
            var objectResult = new ObjectResult(response);
            await objectResult.ExecuteResultAsync(context);
        }
    }
}