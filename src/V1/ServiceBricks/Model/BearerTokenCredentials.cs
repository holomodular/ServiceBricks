namespace ServiceBricks
{
    /// <summary>
    /// This is a bearer token credential.
    /// </summary>
    public class BearerTokenCredentials
    {
        public string AuthorizationUrl { get; set; }
        public AccessTokenRequest AccessTokenRequest { get; set; }
        public AccessTokenResponse AccessTokenResponse { get; set; }
    }
}