namespace ServiceBricks
{
    /// <summary>
    /// This is an application email data transfer object.
    /// </summary>
    public partial class ApplicationEmailDto : DataTransferObject
    {
        /// <summary>
        /// The future process date.
        /// </summary>
        public virtual DateTimeOffset? FutureProcessDate { get; set; }

        /// <summary>
        /// The email type.
        /// </summary>
        public virtual bool IsHtml { get; set; }

        /// <summary>
        /// The priority of the email.
        /// </summary>
        public virtual string Priority { get; set; }

        /// <summary>
        /// The subject of the email.
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        /// The BCC address.
        /// </summary>
        public virtual string BccAddress { get; set; }

        /// <summary>
        /// The CC address.
        /// </summary>
        public virtual string CcAddress { get; set; }

        /// <summary>
        /// The to address.
        /// </summary>
        public virtual string ToAddress { get; set; }

        /// <summary>
        /// The from address.
        /// </summary>
        public virtual string FromAddress { get; set; }

        /// <summary>
        /// The body of the email.
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// The body of the email in HTML format.
        /// </summary>
        public virtual string BodyHtml { get; set; }
    }
}