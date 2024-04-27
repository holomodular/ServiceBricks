namespace ServiceBricks
{
    /// <summary>
    /// This is an application email data transfer object.
    /// </summary>
    public class ApplicationEmailDto : DataTransferObject
    {
        public DateTimeOffset? FutureProcessDate { get; set; }
        public virtual bool IsHtml { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Subject { get; set; }
        public virtual string BccAddress { get; set; }
        public virtual string CcAddress { get; set; }
        public virtual string ToAddress { get; set; }
        public virtual string FromAddress { get; set; }
        public virtual string Body { get; set; }
        public virtual string BodyHtml { get; set; }
    }
}