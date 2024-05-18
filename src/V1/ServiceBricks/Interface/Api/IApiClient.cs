namespace ServiceBricks
{
    /// <summary>
    /// This is a rest-based client interface to call an api service.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IApiClient<TDto> : IApiService<TDto>
        where TDto : class
    {
        string BaseUrl { get; set; }
        string ApiResource { get; set; }
    }
}