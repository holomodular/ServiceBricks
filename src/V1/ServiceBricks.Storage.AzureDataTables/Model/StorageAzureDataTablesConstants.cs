namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// These are constants used by the Azure Data Tables storage provider.
    /// </summary>
    public partial class StorageAzureDataTablesConstants
    {
        /// <summary>
        /// Application setting for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Storage:AzureDataTables:ConnectionString";

        /// <summary>
        /// Minimum date for DateTimeOffset.
        /// </summary>
        public static DateTimeOffset DATETIMEOFFSET_MINDATE = new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Minimum date for DateTimeOffset.
        /// </summary>
        public static DateTime DATETIME_MINDATE = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The delimiter for the storage key.
        /// </summary>
        public const string STORAGEKEY_DELIMITER = "|";

        /// <summary>
        /// The delimiter for the key.
        /// </summary>
        public const string KEY_DELIMITER = "_";

        /// <summary>
        /// The maximum page size for a query.
        /// </summary>
        public const int MAX_QUERY_PAGE_SIZE = 1000;
    }
}