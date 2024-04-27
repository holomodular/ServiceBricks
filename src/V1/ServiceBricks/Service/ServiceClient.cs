namespace ServiceBricks
{
    /// <summary>
    /// This is a REST-based service client.
    /// </summary>
    public class ServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public const string HEADER_CONTENTTYPE = "Content-Type";
        public const string CONTENTTYPE_APPLICATIONJSON = "application/json";

        public ServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public virtual string NamedClient { get; set; }

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            if (request == null)
                return null;
            using (HttpClient client = CreateClient())
                return client.Send(request);
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null)
                return null;
            using (HttpClient client = CreateClient())
                return await client.SendAsync(request);
        }

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