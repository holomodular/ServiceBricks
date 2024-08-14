using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This is the API service interface.
    /// </summary>
    public partial interface IApiService
    {
    }

    /// <summary>
    /// This is the API service for a Dto object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IApiService<TDto> : IApiService
        where TDto : class
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        IResponseItem<TDto> Get(string storageKey);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        Task<IResponseItem<TDto>> GetAsync(string storageKey);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IResponseItem<TDto> Update(TDto dto);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResponseItem<TDto>> UpdateAsync(TDto dto);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IResponseItem<TDto> Create(TDto dto);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResponseItem<TDto>> CreateAsync(TDto dto);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        IResponse Delete(string storageKey);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        Task<IResponse> DeleteAsync(string storageKey);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="serviceQueryRequest"></param>
        /// <returns></returns>
        IResponseItem<ServiceQueryResponse<TDto>> Query(ServiceQueryRequest serviceQueryRequest);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="serviceQueryRequest"></param>
        /// <returns></returns>
        Task<IResponseItem<ServiceQueryResponse<TDto>>> QueryAsync(ServiceQueryRequest serviceQueryRequest);
    }
}