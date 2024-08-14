namespace ServiceBricks
{
    /// <summary>
    /// This is a bearer token credential.
    /// </summary>
    public partial class BearerTokenCredentials
    {
        /// <summary>
        /// The url of the authorization.
        /// </summary>
        public virtual string AuthorizationUrl { get; set; }

        /// <summary>
        /// The access token request.
        /// </summary>
        public virtual AccessTokenRequest AccessTokenRequest { get; set; }

        /// <summary>
        /// The access token response.
        /// </summary>
        public virtual AccessTokenResponse AccessTokenResponse { get; set; }
    }
}