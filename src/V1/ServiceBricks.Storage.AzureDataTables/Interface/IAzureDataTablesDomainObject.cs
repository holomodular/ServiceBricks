using Azure.Data.Tables;

namespace ServiceBricks.Storage.AzureDataTables
{
    public interface IAzureDataTablesDomainObject<TDomain> : IDomainObject<TDomain>, ITableEntity
    {
    }
}