namespace ServiceBricks
{
    /// <summary>
    /// This is an authentication token request.
    /// </summary>
    public class AccessTokenRequest
    {
        public AccessTokenRequest()
        {
            //grant_type = "client_credentials";
        }

        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
        public string response_type { get; set; }
    }
}