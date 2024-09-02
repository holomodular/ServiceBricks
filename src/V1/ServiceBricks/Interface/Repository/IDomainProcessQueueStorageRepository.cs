namespace ServiceBricks
{
    /// <summary>
    /// This is a storage repository that gets records in a queue fashion from a table.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface IDomainProcessQueueStorageRepository<TDomainObject> : IStorageRepository<TDomainObject>
        where TDomainObject : class, IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Get the queue items.
        /// </summary>
        /// <param name="batchNumberToTake"></param>
        /// <param name="pickupErrors"></param>
        /// <param name="errorPickupCutoffDate"></param>
        /// <returns></returns>
        Task<IResponseList<TDomainObject>> GetQueueItemsAsync(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate);
    }
}