namespace ServiceBricks
{
    /// <summary>
    /// This is REST API configurations.
    /// </summary>
    public partial class ClientApiOptions
    {
        /// <summary>
        /// The base service url.
        /// </summary>
        public virtual string BaseServiceUrl { get; set; }

        /// <summary>
        /// The token url.
        /// </summary>
        public virtual string TokenUrl { get; set; }

        /// <summary>
        /// The token type.
        /// </summary>
        public virtual string TokenType { get; set; }

        /// <summary>
        /// The token client.
        /// </summary>
        public virtual string TokenClient { get; set; }

        /// <summary>
        /// The token secret.
        /// </summary>
        public virtual string TokenSecret { get; set; }

        /// <summary>
        /// The token response type.
        /// </summary>
        public virtual string TokenResponseType { get; set; }

        /// <summary>
        /// The token scope.
        /// </summary>
        public virtual string TokenScope { get; set; }

        /// <summary>
        /// Determines if the response object should be returned.
        /// </summary>
        public virtual bool ReturnResponseObject { get; set; }

        /// <summary>
        /// Determines if authentication should be disabled.
        /// </summary>
        public virtual bool DisableAuthentication { get; set; }
    }
}