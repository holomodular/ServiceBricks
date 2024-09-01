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
            Name = "ServiceBricks";
        }

        /// <summary>
        /// The application name
        /// </summary>
        public virtual string Name { get; set; }
    }
}