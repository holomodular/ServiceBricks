using AutoMapper;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// Resolve the partition key.
    /// </summary>
    public partial class PartitionKeyResolver : IValueResolver<DataTransferObject, object, string>
    {
        /// <summary>
        /// Resolve the partition key.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="sourceMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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