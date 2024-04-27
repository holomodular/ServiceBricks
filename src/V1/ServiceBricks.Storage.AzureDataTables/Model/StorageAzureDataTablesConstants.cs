namespace ServiceBricks.Storage.AzureDataTables
{
    public class StorageAzureDataTablesConstants
    {
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Storage:AzureDataTables:ConnectionString";

        public static DateTimeOffset DATETIMEOFFSET_MINDATE = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public const string STORAGEKEY_DELIMITER = "|";

        public const string KEY_DELIMITER = "_";

        public const int MAX_QUERY_PAGE_SIZE = 1000;
    }
}