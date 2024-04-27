namespace ServiceBricks
{
    /// <summary>
    /// This is a storage repository that gets records in a queue fashion from a table.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface IDomainObjectProcessQueueStorageRepository<TDomainObject> : IStorageRepository<TDomainObject>
        where TDomainObject : class, IDomainObject<TDomainObject>
    {
        Task<IResponseList<TDomainObject>> GetQueueItemsAsync(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate);
    }
}