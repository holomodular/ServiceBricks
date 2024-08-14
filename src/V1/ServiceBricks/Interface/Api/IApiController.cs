using Microsoft.AspNetCore.Mvc;
using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// API controller interface.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IApiController<TDto>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActionResult Create(TDto dto);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ActionResult> CreateAsync(TDto dto);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActionResult Update(TDto dto);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ActionResult> UpdateAsync(TDto dto);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        ActionResult Delete(string storageKey);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteAsync(string storageKey);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        ActionResult Get(string storageKey);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        Task<ActionResult> GetAsync(string storageKey);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ActionResult Query(ServiceQueryRequest request);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ActionResult> QueryAsync(ServiceQueryRequest request);

        /// <summary>
        /// Get error response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        ActionResult GetErrorResponse(IResponse response);
    }
}