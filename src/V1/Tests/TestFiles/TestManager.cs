using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public abstract class TestManager<TDto> : ITestManager<TDto>
        where TDto : class, IDataTransferObject, new()
    {
        public virtual TDto GetMinimumDataObject()
        {
            return new TDto();
        }

        public virtual TDto GetObjectNotFound()
        {
            return new TDto()
            {
                StorageKey = Guid.NewGuid().ToString()
            };
        }

        public virtual TDto FindObject(List<TDto> list, TDto dto)
        {
            return list.Where(x => x.StorageKey == dto.StorageKey).FirstOrDefault();
        }

        public abstract TDto GetMaximumDataObject();

        public abstract IApiController<TDto> GetController(IServiceProvider serviceProvider);

        public abstract IApiController<TDto> GetControllerReturnResponse(IServiceProvider serviceProvider);

        public abstract IApiClient<TDto> GetClient(IServiceProvider serviceProvider);

        public abstract IApiClient<TDto> GetClientReturnResponse(IServiceProvider serviceProvider);

        public abstract IApiService<TDto> GetService(IServiceProvider serviceProvider);

        public abstract void UpdateObject(TDto dto);

        public abstract void ValidateObjects(TDto clientDto, TDto serviceDto, HttpMethod method);

        public abstract List<ServiceQueryRequest> GetQueriesForObject(TDto dto);
    }
}