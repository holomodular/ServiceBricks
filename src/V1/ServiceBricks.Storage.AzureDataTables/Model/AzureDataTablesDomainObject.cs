using Azure;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract partial class AzureDataTablesDomainObject<TDomainObject> : DomainObject<TDomainObject>, IAzureDataTablesDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AzureDataTablesDomainObject()
        {
            PartitionKey = string.Empty;
            RowKey = string.Empty;
        }

        /// <summary>
        /// The partition key is a unique identifier for the partition within a given table and forms the first part of an entity's primary key.
        /// </summary>
        public virtual string PartitionKey { get; set; }

        /// <summary>
        ///     The row key is a unique identifier for an entity within a given partition. Together
        ///     the Azure.Data.Tables.ITableEntity.PartitionKey and RowKey uniquely identify
        ///     every entity within a table.
        /// </summary>
        public virtual string RowKey { get; set; }

        /// <summary>
        ///     The Timestamp property is a DateTime value that is maintained on the server side
        ///     to record the time an entity was last modified. The Table service uses the Timestamp
        ///     property internally to provide optimistic concurrency. The value of Timestamp
        ///     is a monotonically increasing value, meaning that each time the entity is modified,
        ///     the value of Timestamp increases for that entity. This property should not be
        ///     set on insert or update operations (the value will be ignored).
        /// </summary>
        public virtual DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the entity's ETag.
        /// </summary>
        public virtual ETag ETag { get; set; }
    }
}