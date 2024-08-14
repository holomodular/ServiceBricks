namespace ServiceBricks
{
    /// <summary>
    /// This is a data transfer object.
    /// </summary>
    public partial class DataTransferObject : IDataTransferObject
    {
        /// <summary>
        /// The storage key.
        /// </summary>
        public virtual string StorageKey { get; set; }
    }
}