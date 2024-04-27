using AutoMapper;

namespace ServiceBricks.Storage.AzureDataTables
{
    public class PartitionKeyResolver : IValueResolver<DataTransferObject, object, string>
    {
        public string Resolve(DataTransferObject source, object destination, string sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.StorageKey))
                return string.Empty;

            string[] split = source.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
            if (split.Length >= 1)
                return split[0];
            return string.Empty;
        }
    }
}