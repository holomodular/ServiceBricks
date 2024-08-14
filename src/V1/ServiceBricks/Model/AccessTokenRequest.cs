namespace ServiceBricks
{
    /// <summary>
    /// This is an authentication token request.
    /// </summary>
    public partial class AccessTokenRequest
    {
        public AccessTokenRequest()
        {
            //grant_type = "client_credentials";
        }

        /// <summary>
        /// The client id.
        /// </summary>
        public virtual string client_id { get; set; }

        /// <summary>
        /// The grant type.
        /// </summary>
        public virtual string grant_type { get; set; }

        /// <summary>
        /// The client secret.
        /// </summary>
        public virtual string client_secret { get; set; }

        /// <summary>
        /// The scope.
        /// </summary>
        public virtual string scope { get; set; }

        /// <summary>
        /// The response type.
        /// </summary>
        public virtual string response_type { get; set; }
    }
}