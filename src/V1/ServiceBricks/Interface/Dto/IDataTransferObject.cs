namespace ServiceBricks
{
    /// <summary>
    /// Defines the interface for a data transfer object.
    /// </summary>
    public partial interface IDataTransferObject
    {
        /// <summary>
        /// Gets or sets the storage key.
        /// </summary>
        string StorageKey { get; set; }
    }

    /// <summary>
    /// Defines the interface for a data transfer object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IDataTransferObject<TDto> : IDataTransferObject
    {
    }
}