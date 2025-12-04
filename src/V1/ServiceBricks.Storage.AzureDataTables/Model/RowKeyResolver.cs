//using AutoMapper;

//namespace ServiceBricks.Storage.AzureDataTables
//{
//    /// <summary>
//    /// Resolve the row key.
//    /// </summary>
//    public partial class RowKeyResolver : IValueResolver<DataTransferObject, object, string>
//    {
//        /// <summary>
//        /// Resolve the row key.
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="destination"></param>
//        /// <param name="sourceMember"></param>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string Resolve(DataTransferObject source, object destination, string sourceMember, ResolutionContext context)
//        {
//            if (string.IsNullOrEmpty(source.StorageKey))
//                return string.Empty;

//            string[] split = source.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
//            if (split.Length >= 2)
//                return split[1];
//            return string.Empty;
//        }
//    }
//}