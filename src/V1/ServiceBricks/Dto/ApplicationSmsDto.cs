namespace ServiceBricks
{
    /// <summary>
    /// This is an application Sms data transfer object.
    /// </summary>
    public class ApplicationSmsDto : DataTransferObject
    {
        public DateTimeOffset? FutureProcessDate { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Message { get; set; }
    }
}