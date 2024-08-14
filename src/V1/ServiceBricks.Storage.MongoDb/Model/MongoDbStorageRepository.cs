using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// This storage repository uses Azure Data Tables to provide specific functions.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public partial class MongoDbStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class, IMongoDbDomainObject<TDomain>, new()
    {
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logFactory"></param>
        /// <param name="businessRuleService"></param>
        public MongoDbStorageRepository(
            ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<MongoDbStorageRepository<TDomain>>();
        }

        /// <summary>
        /// The connection string to the MongoDb database.
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database.
        /// </summary>
        public virtual string DatabaseName { get; set; }

        /// <summary>
        /// The name of the collection.
        /// </summary>
        public virtual string CollectionName { get; set; }

        /// <summary>
        /// Determines if service query errors are logged.
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var exp = obj.DomainGetItemFilter(obj);
                collection.DeleteOne(exp);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var exp = obj.DomainGetItemFilter(obj);
                await collection.DeleteOneAsync(exp);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                collection.InsertOne(obj);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                await collection.InsertOneAsync(obj);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var filter = obj.DomainGetItemFilter(obj);
                collection.ReplaceOne(filter, obj);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var filter = obj.DomainGetItemFilter(obj);
                await collection.ReplaceOneAsync(filter, obj);
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var filter = obj.DomainGetItemFilter(obj);
                var query = collection.Find(filter);
                response.Item = query.FirstOrDefault();
                return response;
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
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var filter = obj.DomainGetItemFilter(obj);
                var query = await collection.FindAsync(filter);
                response.Item = await query.FirstOrDefaultAsync();
                return response;
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
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "request"));
                    return response;
                }
                MongoClient client = new MongoClient(ConnectionString);
                var db = client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<TDomain>(CollectionName);
                var queryable = collection.AsQueryable();
                response.Item = request.Execute(queryable);
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