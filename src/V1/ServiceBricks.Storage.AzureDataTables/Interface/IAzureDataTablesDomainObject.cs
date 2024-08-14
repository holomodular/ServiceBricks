using Azure.Data.Tables;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// Defines the interface for an Azure Data Tables domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IAzureDataTablesDomainObject<TDomain> : IDomainObject<TDomain>, ITableEntity
    {
    }
}