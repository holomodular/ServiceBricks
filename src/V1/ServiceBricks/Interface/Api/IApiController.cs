using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// API controller interface.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IApiController<TDto>
        where TDto : class
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
        Task<ActionResult> CreateAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActionResult CreateAck(TDto dto);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ActionResult> CreateAckAsync(TDto dto, CancellationToken cancellationToken = default);

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
        Task<ActionResult> UpdateAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActionResult UpdateAck(TDto dto);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ActionResult> UpdateAckAsync(TDto dto, CancellationToken cancellationToken = default);

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
        Task<ActionResult> DeleteAsync(string storageKey, CancellationToken cancellationToken = default);

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
        Task<ActionResult> GetAsync(string storageKey, CancellationToken cancellationToken = default);

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
        Task<ActionResult> QueryAsync(ServiceQueryRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        ActionResult Patch(string storageKey, JsonPatchDocument<TDto> patchDocument);

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        Task<ActionResult> PatchAsync(string storageKey, JsonPatchDocument<TDto> patchDocument, CancellationToken cancellationToken = default);

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        ActionResult PatchAck(string storageKey, JsonPatchDocument<TDto> patchDocument);

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        Task<ActionResult> PatchAckAsync(string storageKey, JsonPatchDocument<TDto> patchDocument, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActionResult Validate(TDto dto);

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActionResult> ValidateAsync(TDto dto, CancellationToken cancellationToken = default);
    }
}