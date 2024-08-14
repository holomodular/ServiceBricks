namespace ServiceBricks
{
    /// <summary>
    /// This is a REST-based service client.
    /// </summary>
    public partial class ServiceClient
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constant for the Content-Type header.
        /// </summary>
        public const string HEADER_CONTENTTYPE = "Content-Type";

        /// <summary>
        /// Application/JSON content type.
        /// </summary>
        public const string CONTENTTYPE_APPLICATIONJSON = "application/json";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public ServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// The named client to use.
        /// </summary>
        public virtual string NamedClient { get; set; }

        /// <summary>
        /// Send a request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            if (request == null)
                return null;
            using (HttpClient client = CreateClient())
                return client.Send(request);
        }

        /// <summary>
        /// Send a request asynchronously
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null)
                return null;
            using (HttpClient client = CreateClient())
                return await client.SendAsync(request);
        }

        /// <summary>
        /// Create a client
        /// </summary>
        /// <returns></returns>
        public virtual HttpClient CreateClient()
        {
            HttpClient client = null;
            if (string.IsNullOrEmpty(NamedClient))
                client = _httpClientFactory.CreateClient();
            else
                client = _httpClientFactory.CreateClient(NamedClient);
            return client;
        }
    }
}