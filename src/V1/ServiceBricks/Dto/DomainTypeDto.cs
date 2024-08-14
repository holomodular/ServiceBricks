namespace ServiceBricks
{
    /// <summary>
    /// This is a domain type data transfer object.
    /// </summary>
    public partial class DomainTypeDto : DataTransferObject
    {
        /// <summary>
        /// The name.
        /// </summary>
        public virtual string Name { get; set; }
    }
}