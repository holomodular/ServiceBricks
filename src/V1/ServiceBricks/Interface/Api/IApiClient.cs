namespace ServiceBricks
{
    /// <summary>
    /// This is a rest-based client interface to call an api service.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IApiClient<TDto> : IApiService<TDto>
        where TDto : class
    {
        /// <summary>
        /// The base url for the api.
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// The api resource.
        /// </summary>
        string ApiResource { get; set; }
    }
}