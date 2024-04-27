namespace ServiceBricks
{
    public interface IDataTransferObject
    {
        string StorageKey { get; set; }
    }

    public interface IDataTransferObject<TDto> : IDataTransferObject
    {
    }
}