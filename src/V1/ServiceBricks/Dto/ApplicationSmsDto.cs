namespace ServiceBricks
{
    /// <summary>
    /// This is an application Sms data transfer object.
    /// </summary>
    public partial class ApplicationSmsDto : DataTransferObject
    {
        /// <summary>
        /// The future process date.
        /// </summary>
        public virtual DateTimeOffset? FutureProcessDate { get; set; }

        /// <summary>
        /// The phone number.
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// The message
        /// </summary>
        public virtual string Message { get; set; }
    }
}