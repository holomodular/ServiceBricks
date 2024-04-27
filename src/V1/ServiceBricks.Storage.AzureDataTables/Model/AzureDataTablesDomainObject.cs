using Azure;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract class AzureDataTablesDomainObject<TDomainObject> : DomainObject<TDomainObject>, IAzureDataTablesDomainObject<TDomainObject>
    {
        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }
    }
}