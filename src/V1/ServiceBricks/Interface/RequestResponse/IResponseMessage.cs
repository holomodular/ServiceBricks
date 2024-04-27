namespace ServiceBricks
{
    /// <summary>
    /// This is a response message.
    /// </summary>
    public interface IResponseMessage
    {
        /// <summary>
        /// Severity of the message.
        /// </summary>
        ResponseSeverity Severity { get; set; }

        /// <summary>
        /// The message displayed to the user.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// The field(s) this messages correlates to.
        /// </summary>
        System.Collections.Generic.IList<string> Fields { get; set; }
    }
}