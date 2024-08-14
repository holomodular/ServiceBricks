using AutoMapper;
using Azure.Data.Tables;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// Resolve the storage key.
    /// </summary>
    public partial class StorageKeyResolver : IValueResolver<ITableEntity, DataTransferObject, string>
    {
        /// <summary>
        /// Resolve the storage key.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="sourceMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Resolve(ITableEntity source, DataTransferObject destination, string sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.RowKey))
                return source.PartitionKey;

            return source.PartitionKey +
                StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                source.RowKey;
        }
    }
}