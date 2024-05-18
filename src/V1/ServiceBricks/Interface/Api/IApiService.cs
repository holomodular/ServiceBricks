using ServiceQuery;

namespace ServiceBricks
{
    public interface IApiService
    {
    }

    /// <summary>
    /// This is the API service for a Dto object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IApiService<TDto> : IApiService
        where TDto : class
    {
        IResponseItem<TDto> Get(string storageKey);

        Task<IResponseItem<TDto>> GetAsync(string storageKey);

        IResponseItem<TDto> Update(TDto dto);

        Task<IResponseItem<TDto>> UpdateAsync(TDto dto);

        IResponseItem<TDto> Create(TDto dto);

        Task<IResponseItem<TDto>> CreateAsync(TDto dto);

        IResponse Delete(string storageKey);

        Task<IResponse> DeleteAsync(string storageKey);

        IResponseItem<ServiceQueryResponse<TDto>> Query(ServiceQueryRequest serviceQueryRequest);

        Task<IResponseItem<ServiceQueryResponse<TDto>>> QueryAsync(ServiceQueryRequest serviceQueryRequest);
    }
}