using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This storage repository uses the Entity Framework ORM to provide database specific functions.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public partial class EntityFrameworkCoreStorageRepository<TDomain> : IEntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        protected ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logFactory"></param>
        /// <param name="businessRuleService"></param>
        public EntityFrameworkCoreStorageRepository(ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<EntityFrameworkCoreStorageRepository<TDomain>>();
        }

        /// <summary>
        /// The database set.
        /// </summary>
        public virtual DbSet<TDomain> DbSet { get; set; }

        /// <summary>
        /// The database context.
        /// </summary>
        public virtual DbContext Context { get; set; }

        /// <summary>
        /// The ServiceQueryOptions.
        /// </summary>
        public virtual ServiceQueryOptions ServiceQueryOptions { get; set; }

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
        /// Detach all entities.
        /// </summary>
        public void DetachAllEntities()
        {
            var changedEntriesCopy = Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        /// <returns></returns>
        public virtual IResponse SaveChanges()
        {
            var resp = new Response();
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SaveChanges)} {ex.Message}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponse> SaveChangesAsync()
        {
            var resp = new Response();
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SaveChangesAsync)} {ex.Message}");
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return resp;
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
                var getitem = obj.DomainGetItemFilter(obj);
                var existing = DbSet.Local.AsQueryable().Where(getitem).FirstOrDefault();
                if (existing != null)
                    Context.Entry(existing).State = EntityState.Detached;

                DbSet.Remove(obj);

                Context.SaveChanges();
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
                var getitem = obj.DomainGetItemFilter(obj);
                var existing = DbSet.Local.AsQueryable().Where(getitem).FirstOrDefault();
                if (existing != null)
                    Context.Entry(existing).State = EntityState.Detached;

                DbSet.Remove(obj);

                await Context.SaveChangesAsync();
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
                DbSet.Add(obj);

                Context.SaveChanges();
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
                await DbSet.AddAsync(obj);

                await Context.SaveChangesAsync();
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
                var getItemExpression = obj.DomainGetItemFilter(obj);
                var existing = DbSet.Local.AsQueryable().Where(getItemExpression).FirstOrDefault();
                if (existing != null)
                    Context.Entry(existing).State = EntityState.Detached;
                DbSet.Attach(obj).State = EntityState.Modified;

                Context.SaveChanges();
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
                var getItemExpression = obj.DomainGetItemFilter(obj);
                var existing = DbSet.Local.AsQueryable().Where(getItemExpression).FirstOrDefault();
                if (existing != null)
                    Context.Entry(existing).State = EntityState.Detached;
                DbSet.Attach(obj).State = EntityState.Modified;

                await Context.SaveChangesAsync();
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
                var query = DbSet.AsQueryable<TDomain>();
                query = obj.DomainGetIQueryableDefaults(query);

                var getItemExpression = obj.DomainGetItemFilter(obj);
                response.Item = query.Where(getItemExpression).AsNoTracking().FirstOrDefault();
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
        /// GetItem domain objects.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDomain>> GetAsync(TDomain obj)
        {
            var response = new ResponseItem<TDomain>();
            try
            {
                var query = DbSet.AsQueryable<TDomain>();
                query = obj.DomainGetIQueryableDefaults(query);

                var getItemExpression = obj.DomainGetItemFilter(obj);
                response.Item = await query.Where(getItemExpression).AsNoTracking().FirstOrDefaultAsync();
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
        /// <returns></returns>
        public virtual IQueryable<TDomain> GetQueryable()
        {
            try
            {
                return DbSet.AsQueryable<TDomain>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetQueryable));
            }
            return null;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<ServiceQueryResponse<TDomain>> Query(ServiceQueryRequest request)
        {
            ResponseItem<ServiceQueryResponse<TDomain>> response = new ResponseItem<ServiceQueryResponse<TDomain>>();
            try
            {
                if (request == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                    return response;
                }
                var queryable = GetQueryable();
                response.Item = request.Execute(queryable, ServiceQueryOptions);
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
        public virtual async Task<IResponseItem<ServiceQueryResponse<TDomain>>> QueryAsync(ServiceQueryRequest request)
        {
            ResponseItem<ServiceQueryResponse<TDomain>> response = new ResponseItem<ServiceQueryResponse<TDomain>>();
            try
            {
                if (request == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                    return response;
                }
                var queryable = GetQueryable();
                response.Item = await request.ExecuteAsync(queryable, ServiceQueryOptions);
                return response;
            }
            catch (ServiceQueryException sqe)
            {
                if (LogServiceQueryErrors)
                    _logger.LogError(sqe, $"{nameof(QueryAsync)} {sqe.Message} {JsonConvert.SerializeObject(request)}");
                response.AddMessage(ResponseMessage.CreateError(sqe, LocalizationResource.ERROR_SERVICEQUERY));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(QueryAsync)} {ex.Message} {JsonConvert.SerializeObject(request)}");
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_STORAGE));
            }
            return response;
        }
    }
}