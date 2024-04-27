namespace ServiceBricks
{
    /// <summary>
    /// Enumeration of response message severities.
    /// </summary>
    public enum ResponseSeverity : int
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 0,

        /// <summary>
        /// Information
        /// </summary>
        Info = 1,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Error
        /// </summary>
        Error = 3,

        /// <summary>
        /// System Error (may contain sensitive information)
        /// </summary>
        ErrorSystemSensitive = 4,
    }
}