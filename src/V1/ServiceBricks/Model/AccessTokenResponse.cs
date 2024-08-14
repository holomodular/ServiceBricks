namespace ServiceBricks
{
    /// <summary>
    /// This is an authentication token response.
    /// </summary>
    public partial class AccessTokenResponse
    {
        /// <summary>
        /// The scope
        /// </summary>
        public virtual string scope { get; set; }

        /// <summary>
        /// The access token
        /// </summary>
        public virtual string access_token { get; set; }

        /// <summary>
        /// The token type
        /// </summary>
        public virtual string token_type { get; set; }

        /// <summary>
        /// The app id
        /// </summary>
        public virtual string app_id { get; set; }

        /// <summary>
        /// Expires in
        /// </summary>
        public virtual int expires_in { get; set; }

        /// <summary>
        /// Nonce
        /// </summary>
        public virtual string nonce { get; set; }
    }
}