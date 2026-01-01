namespace ServiceBricks
{
    /// <summary>
    /// This is a response.
    /// </summary>
    public partial interface IResponse
    {
        /// <summary>
        /// Determine if the response is successful or if an error happened.
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Determine if the response is successful or if an error happened.
        /// </summary>
        bool Error { get; set; }

        /// <summary>
        /// The collection of response messages.
        /// </summary>
        IReadOnlyList<IResponseMessage> Messages { get; }

        /// <summary>
        /// Copy from response object to this instance.
        /// </summary>
        /// <param name="from"></param>
        void CopyFrom(IResponse from);

        /// <summary>
        /// Copy this object to the response object.
        /// </summary>
        /// <param name="to"></param>
        void CopyTo(IResponse to);

        /// <summary>
        /// Add a message to the response. If the message is an error, it will also set the response.Success to false as well.
        /// </summary>
        /// <param name="message"></param>
        void AddMessage(IResponseMessage message);

        /// <summary>
        /// Add a list of messages to the response. If amessage is an error, it will also set the response.Success to false as well.
        /// </summary>
        /// <param name="message"></param>
        void AddMessage(List<IResponseMessage> messages);

        /// <summary>
        /// Get all the messages as a single string.
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        string GetMessage(string seperator);

        /// <summary>
        /// Scrub the response to remove any sensitive system errors.
        /// </summary>
        void Scrub();
    }
}