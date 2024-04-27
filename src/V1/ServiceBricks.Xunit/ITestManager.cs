using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public interface ITestManager<TDto>
        where TDto : class, IDataTransferObject
    {
        public TDto GetMinimumDataObject();

        public TDto GetMaximumDataObject();

        public TDto GetObjectNotFound();

        public IApiClient<TDto> GetClient(IServiceProvider serviceProvider);

        public IApiClient<TDto> GetClientReturnResponse(IServiceProvider serviceProvider);

        public IApiController<TDto> GetController(IServiceProvider serviceProvider);

        public IApiController<TDto> GetControllerReturnResponse(IServiceProvider serviceProvider);

        public void ValidateObjects(TDto localDto, TDto serviceDto, HttpMethod method);

        public void UpdateObject(TDto localDto);

        public TDto FindObject(List<TDto> list, TDto dto);

        public List<ServiceQueryRequest> GetQueriesForObject(TDto dto);
    }
}