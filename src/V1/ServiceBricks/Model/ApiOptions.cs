namespace ServiceBricks
{
    /// <summary>
    /// Options for the API.
    /// </summary>
    public partial class ApiOptions
    {
        /// <summary>
        /// Determines if the API should expose ErrorSystemSensitive severity errors.
        /// </summary>
        public virtual bool ExposeSystemErrors { get; set; }

        /// <summary>
        /// Determines if the API should return a response object with the response, classic vs modern design.
        /// </summary>
        public virtual bool ReturnResponseObject { get; set; }
    }
}