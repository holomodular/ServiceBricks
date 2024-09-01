using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This storage repository uses Azure Data Tables to provide specific functions.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class AzureDataTablesStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class, IAzureDataTablesDomainObject<TDomain>, new()
    {
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AzureDataTablesStorageRepository(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AzureDataTablesStorageRepository<TDomain>>();
        }

        /// <summary>
        /// The ServiceQueryOptions.
        /// </summary>
        public virtual ServiceQueryOptions ServiceQueryOptions { get; set; }

        /// <summary>
        /// The AzureDataTablesOptions.
        /// </summary>
        public virtual AzureDataTablesOptions AzureDataTablesOptions { get; set; }

        /// <summary>
        /// The connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// The table name
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// Log service query errors.
        /// </summary>
        public virtual bool LogServiceQueryErrors { get; set; }

        /// <summary>
        /// Get the storage repository.
        /// </summary>
        /// <returns></returns>
        public virtual IStorageRepository<TDomain> GetStorageRepository()
        {
            return this;
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IResponse Delete(TDomain obj)
        {
            var resp = new Response();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                tableClient.DeleteEntity(obj.PartitionKey, obj.RowKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Delete)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> DeleteAsync(TDomain obj)
        {
            var resp = new Response();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                await tableClient.DeleteEntityAsync(obj.PartitionKey, obj.RowKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteAsync)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IResponse Create(TDomain obj)
        {
            Response resp = new Response();

            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                tableClient.AddEntity(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Create)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> CreateAsync(TDomain obj)
        {
            Response resp = new Response();

            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                await tableClient.AddEntityAsync(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CreateAsync)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IResponse Update(TDomain obj)
        {
            var resp = new Response();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                tableClient.UpdateEntity(obj, ETag.All);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Update)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> UpdateAsync(TDomain obj)
        {
            var resp = new Response();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                await tableClient.UpdateEntityAsync(obj, ETag.All);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateAsync)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// GetItem domain objects.
        /// </summary>
        /// <returns></returns>
        public virtual IResponseItem<TDomain> Get(TDomain obj)
        {
            var response = new ResponseItem<TDomain>();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                response.Item = tableClient.GetEntity<TDomain>(obj.PartitionKey, obj.RowKey);
                return response;
            }
            catch (Azure.RequestFailedException rfe)
            {
                if (rfe.ErrorCode == "ResourceNotFound") //no errors if missing
                    return response;
                _logger.LogError(rfe, $"{nameof(Get)} {rfe.Message} {JsonConvert.SerializeObject(obj)}");
                response.AddMessage(ResponseMessage.CreateError(rfe, LocalizationResource.ERROR_STORAGE));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Get)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return response;
        }

        /// <summary>
        /// GetItem domain objects.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDomain>> GetAsync(TDomain obj)
        {
            var response = new ResponseItem<TDomain>();
            try
            {
                TableClient tableClient = new TableClient(ConnectionString, TableName);
                response.Item = await tableClient.GetEntityAsync<TDomain>(obj.PartitionKey, obj.RowKey);
                return response;
            }
            catch (Azure.RequestFailedException rfe)
            {
                if (rfe.ErrorCode == "ResourceNotFound") //no errors if missing
                    return response;
                _logger.LogError(rfe, $"{nameof(GetAsync)} {rfe.Message} {JsonConvert.SerializeObject(obj)}");
                response.AddMessage(ResponseMessage.CreateError(rfe, LocalizationResource.ERROR_STORAGE));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAsync)} {ex.Message} {JsonConvert.SerializeObject(obj)}");
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return response;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<ServiceQueryResponse<TDomain>> Query(ServiceQueryRequest request)
        {
            IResponseItem<ServiceQueryResponse<TDomain>> response = new ResponseItem<ServiceQueryResponse<TDomain>>();
            try
            {
                if (request == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                    return response;
                }

                var tableClient = new TableClient(ConnectionString, TableName);
                response.Item = request.Execute<TDomain>(tableClient, ServiceQueryOptions, AzureDataTablesOptions);
                return response;
            }
            catch (ServiceQueryException sqe)
            {
                if (LogServiceQueryErrors)
                    _logger.LogError(sqe, $"{nameof(Query)} {sqe.Message} {JsonConvert.SerializeObject(request)}");
                response.AddMessage(ResponseMessage.CreateError(sqe, LocalizationResource.ERROR_SERVICEQUERY));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Query)} {ex.Message} {JsonConvert.SerializeObject(request)}");
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return response;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual Task<IResponseItem<ServiceQueryResponse<TDomain>>> QueryAsync(ServiceQueryRequest request)
        {
            return Task.FromResult(Query(request));
        }
    }
}