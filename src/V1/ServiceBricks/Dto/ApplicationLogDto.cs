namespace ServiceBricks
{
    /// <summary>
    /// This is an application log data transer object.
    /// </summary>
    public partial class ApplicationLogDto : DataTransferObject
    {
        /// <summary>
        /// The create date.
        /// </summary>
        public virtual DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The application name.
        /// </summary>
        public virtual string Application { get; set; }

        /// <summary>
        /// The server name.
        /// </summary>
        public virtual string Server { get; set; }

        /// <summary>
        /// The category.
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// The user storage key.
        /// </summary>
        public virtual string UserStorageKey { get; set; }

        /// <summary>
        /// The path.
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// The severity level.
        /// </summary>
        public virtual string Level { get; set; }

        /// <summary>
        /// The message
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// The exception.
        /// </summary>
        public virtual string Exception { get; set; }

        /// <summary>
        /// The properties.
        /// </summary>
        public virtual string Properties { get; set; }
    }
}