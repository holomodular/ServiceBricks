using AutoMapper;
using Azure.Data.Tables;

namespace ServiceBricks.Storage.AzureDataTables
{
    public class StorageKeyResolver : IValueResolver<ITableEntity, DataTransferObject, string>
    {
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