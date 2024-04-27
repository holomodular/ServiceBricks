using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This is the main repository that should be used when interacting with domain objects.
    /// It performs domain object rule processing and storage persistence.
    /// Actions are committed when complete.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public partial class DomainRepository<TDomainObject> : IDomainRepository<TDomainObject>
        where TDomainObject : class, IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Domain rule processing service.
        /// </summary>
        protected IBusinessRuleService _businessRuleService;

        /// <summary>
        /// Storage repository for the object.
        /// </summary>
        protected IStorageRepository<TDomainObject> _storageRepository;

        /// <summary>
        /// Logger.
        /// </summary>
        protected ILogger _logger;

        public DomainRepository(
            ILoggerFactory logFactory,
            IBusinessRuleService businessRuleService,
            IStorageRepository<TDomainObject> storageRepository)
        {
            _logger = logFactory.CreateLogger<DomainRepository<TDomainObject>>();
            _businessRuleService = businessRuleService;
            _storageRepository = storageRepository;
        }

        /// <summary>
        /// Get the underlying storage repository.
        /// </summary>
        /// <returns></returns>
        public virtual IStorageRepository<TDomainObject> GetStorageRepository()
        {
            return _storageRepository;
        }

        /// <summary>
        /// Get a domain object by its key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDomainObject> Get(TDomainObject obj)
        {
            IResponseItem<TDomainObject> response = new ResponseItem<TDomainObject>();

            // Process Before
            DomainGetItemBeforeEvent<TDomainObject> beforeEvent = new DomainGetItemBeforeEvent<TDomainObject>(obj);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get
            var storageResponse = _storageRepository.Get(obj);
            response.CopyFrom(storageResponse);
            response.Item = storageResponse.Item;
            if (!response.Success)
                return response;

            // Process After
            DomainGetItemAfterEvent<TDomainObject> afterEvent = new DomainGetItemAfterEvent<TDomainObject>(response.Item);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Get a domain object by its key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDomainObject>> GetAsync(TDomainObject obj)
        {
            IResponseItem<TDomainObject> response = new ResponseItem<TDomainObject>();

            // Process Before
            DomainGetItemBeforeEvent<TDomainObject> beforeEvent = new DomainGetItemBeforeEvent<TDomainObject>(obj);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get
            var storageResponse = await _storageRepository.GetAsync(obj);
            response.CopyFrom(storageResponse);
            response.Item = storageResponse.Item;
            if (!response.Success)
                return response;

            // Process After
            DomainGetItemAfterEvent<TDomainObject> afterEvent = new DomainGetItemAfterEvent<TDomainObject>(response.Item);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IResponse Create(TDomainObject model)
        {
            IResponse response = new Response();

            // Process generic object Domain rules
            BusinessRuleContext bcontext = new BusinessRuleContext(model);
            IResponse respRules = _businessRuleService.ExecuteRules(bcontext);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Process Before
            DomainCreateBeforeEvent<TDomainObject> beforeEvent = new DomainCreateBeforeEvent<TDomainObject>(model);
            respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Create
            var respI = _storageRepository.Create(model);
            response.CopyFrom(respI);
            if (!response.Success)
                return response;

            // Process After
            DomainCreateAfterEvent<TDomainObject> afterEvent = new DomainCreateAfterEvent<TDomainObject>(model);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> CreateAsync(TDomainObject model)
        {
            IResponse response = new Response();

            // Process generic object Domain rules
            BusinessRuleContext bcontext = new BusinessRuleContext(model);
            IResponse respRules = await _businessRuleService.ExecuteRulesAsync(bcontext);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Process Before
            DomainCreateBeforeEvent<TDomainObject> beforeEvent = new DomainCreateBeforeEvent<TDomainObject>(model);
            respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Create
            var respI = await _storageRepository.CreateAsync(model);
            response.CopyFrom(respI);
            if (!response.Success)
                return response;

            // Process After
            DomainCreateAfterEvent<TDomainObject> afterEvent = new DomainCreateAfterEvent<TDomainObject>(model);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IResponse Update(TDomainObject model)
        {
            IResponse response = new Response();

            // Process generic object Domain rules
            BusinessRuleContext bcontext = new BusinessRuleContext(model);
            IResponse respRules = _businessRuleService.ExecuteRules(bcontext);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Process Before
            DomainUpdateBeforeEvent<TDomainObject> beforeEvent = new DomainUpdateBeforeEvent<TDomainObject>(model);
            respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Update
            var respU = _storageRepository.Update(model);
            response.CopyFrom(respU);
            if (!response.Success)
                return response;

            // Process After
            DomainUpdateAfterEvent<TDomainObject> afterEvent = new DomainUpdateAfterEvent<TDomainObject>(model);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> UpdateAsync(TDomainObject model)
        {
            IResponse response = new Response();

            // Process generic object Domain rules
            BusinessRuleContext bcontext = new BusinessRuleContext(model);
            IResponse respRules = await _businessRuleService.ExecuteRulesAsync(bcontext);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Process Before
            DomainUpdateBeforeEvent<TDomainObject> beforeEvent = new DomainUpdateBeforeEvent<TDomainObject>(model);
            respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Update
            var respU = await _storageRepository.UpdateAsync(model);
            response.CopyFrom(respU);
            if (!response.Success)
                return response;

            // Process After
            DomainUpdateAfterEvent<TDomainObject> afterEvent = new DomainUpdateAfterEvent<TDomainObject>(model);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IResponse Delete(TDomainObject model)
        {
            IResponse response = new Response();

            // Process Before
            DomainDeleteBeforeEvent<TDomainObject> beforeEvent = new DomainDeleteBeforeEvent<TDomainObject>(model);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Delete
            var respD = _storageRepository.Delete(model);
            response.CopyFrom(respD);
            if (!response.Success)
                return response;

            // Process After
            DomainDeleteAfterEvent<TDomainObject> afterEvent = new DomainDeleteAfterEvent<TDomainObject>(model)
            {
                Response = response
            };
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> DeleteAsync(TDomainObject model)
        {
            IResponse response = new Response();

            // Process Before
            DomainDeleteBeforeEvent<TDomainObject> beforeEvent = new DomainDeleteBeforeEvent<TDomainObject>(model);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Delete
            var respD = await _storageRepository.DeleteAsync(model);
            response.CopyFrom(respD);
            if (!response.Success)
                return response;

            // Process After
            DomainDeleteAfterEvent<TDomainObject> afterEvent = new DomainDeleteAfterEvent<TDomainObject>(model)
            {
                Response = response
            };
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponseItem<ServiceQueryResponse<TDomainObject>> Query(ServiceQueryRequest request)
        {
            var response = new ResponseItem<ServiceQueryResponse<TDomainObject>>();

            // Process Before
            DomainQueryBeforeEvent<TDomainObject> beforeEvent = new DomainQueryBeforeEvent<TDomainObject>(request);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get Query
            var result = _storageRepository.Query(request);
            response.CopyFrom(result);
            response.Item = result.Item;
            if (!result.Success)
                return response;

            // Process After
            DomainQueryAfterEvent<TDomainObject> afterEvent = new DomainQueryAfterEvent<TDomainObject>(result);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;
            return response;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IResponseItem<ServiceQueryResponse<TDomainObject>>> QueryAsync(ServiceQueryRequest request)
        {
            var response = new ResponseItem<ServiceQueryResponse<TDomainObject>>();

            // Process Before
            DomainQueryBeforeEvent<TDomainObject> beforeEvent = new DomainQueryBeforeEvent<TDomainObject>(request);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get Query
            var result = await _storageRepository.QueryAsync(request);
            response.CopyFrom(result);
            response.Item = result.Item;
            if (!result.Success)
                return response;

            // Process After
            DomainQueryAfterEvent<TDomainObject> afterEvent = new DomainQueryAfterEvent<TDomainObject>(result);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;
            return response;
        }
    }
}