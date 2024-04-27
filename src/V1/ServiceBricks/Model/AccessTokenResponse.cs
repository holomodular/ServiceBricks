namespace ServiceBricks
{
    /// <summary>
    /// This is an authentication token response.
    /// </summary>
    public class AccessTokenResponse
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public string nonce { get; set; }
    }
}