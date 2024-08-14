namespace ServiceBricks
{
    /// <summary>
    /// Options for the application.
    /// </summary>
    public partial class ApplicationOptions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationOptions()
        {
            Name = "Service Bricks";
            Url = "https://localhost:7000";
        }

        /// <summary>
        /// The application name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The url of the application.
        /// </summary>
        public virtual string Url { get; set; }
    }
}