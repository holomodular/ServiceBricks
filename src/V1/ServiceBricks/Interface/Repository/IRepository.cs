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
        IResponse Create(TDomain model);

        Task<IResponse> CreateAsync(TDomain model);

        IResponse Update(TDomain model);

        Task<IResponse> UpdateAsync(TDomain model);

        IResponse Delete(TDomain model);

        Task<IResponse> DeleteAsync(TDomain model);

        IResponseItem<TDomain> Get(TDomain model);

        Task<IResponseItem<TDomain>> GetAsync(TDomain model);

        IResponseItem<ServiceQueryResponse<TDomain>> Query(ServiceQueryRequest request);

        Task<IResponseItem<ServiceQueryResponse<TDomain>>> QueryAsync(ServiceQueryRequest request);
    }
}