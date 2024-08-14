using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This is the base repository interface.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IRepository<TDomain>
        where TDomain : class
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IResponse Create(TDomain model);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResponse> CreateAsync(TDomain model);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IResponse Update(TDomain model);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResponse> UpdateAsync(TDomain model);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IResponse Delete(TDomain model);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResponse> DeleteAsync(TDomain model);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IResponseItem<TDomain> Get(TDomain model);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResponseItem<TDomain>> GetAsync(TDomain model);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IResponseItem<ServiceQueryResponse<TDomain>> Query(ServiceQueryRequest request);

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResponseItem<ServiceQueryResponse<TDomain>>> QueryAsync(ServiceQueryRequest request);
    }
}