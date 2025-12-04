using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This service is an API for a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TDtoObject"></typeparam>
    public partial class ApiService<TDomainObject, TDtoObject> : IApiService<TDtoObject>
        where TDomainObject : class, IDomainObject<TDomainObject>, new()
        where TDtoObject : class, IDataTransferObject, new()
    {
        protected readonly IDomainRepository<TDomainObject> _repository;
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<TDomainObject> repository)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _repository = repository;
        }

        /// <summary>
        /// Get the underlying repository
        /// </summary>
        /// <returns></returns>
        public virtual IDomainRepository<TDomainObject> GetRepository()
        {
            return _repository;
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDtoObject> Get(string storageKey)
        {
            var response = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (string.IsNullOrEmpty(storageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "storageKey"));
                return response;
            }

            // Map param
            var tempObj = new TDtoObject() { StorageKey = storageKey };
            var tempParam = _mapper.Map<TDtoObject, TDomainObject>(tempObj);

            // Process Before
            ApiGetBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiGetBeforeEvent<TDomainObject, TDtoObject>(storageKey, tempParam);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get from repository
            var respGet = _repository.Get(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            // Map to DTO
            response.Item = _mapper.Map<TDomainObject, TDtoObject>(respGet.Item);

            // Process After
            ApiGetAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiGetAfterEvent<TDomainObject, TDtoObject>(respGet.Item, response.Item);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            return response;
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDtoObject>> GetAsync(string storageKey)
        {
            var response = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (string.IsNullOrEmpty(storageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "storageKey"));
                return response;
            }

            // Map param
            var tempObj = new TDtoObject() { StorageKey = storageKey };
            var tempParam = _mapper.Map<TDtoObject, TDomainObject>(tempObj);

            // Process Before
            ApiGetBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiGetBeforeEvent<TDomainObject, TDtoObject>(storageKey, tempParam);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Get from repository
            var respGet = await _repository.GetAsync(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            // Map to DTO
            response.Item = _mapper.Map<TDomainObject, TDtoObject>(respGet.Item);

            // Process After
            ApiGetAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiGetAfterEvent<TDomainObject, TDtoObject>(respGet.Item, response.Item);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            return response;
        }

        /// <summary>
        /// Update an item.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDtoObject> Update(TDtoObject dto)
        {
            var response = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (dto == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "dto"));
                return response;
            }

            // Process generic Dto object rules
            BusinessRuleContext bcontext = new BusinessRuleContext(dto);
            IResponse bizResp = _businessRuleService.ExecuteRules(bcontext);
            response.CopyFrom(bizResp);
            if (!response.Success)
                return response;

            // GetItem
            var tempParam = _mapper.Map<TDtoObject, TDomainObject>(dto);
            var respGet = _repository.Get(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            if (respGet.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // Process Before
            ApiUpdateBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiUpdateBeforeEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respBeforeRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respBeforeRules);
            if (!response.Success)
                return response;

            // Map Dto to Domain
            _mapper.Map<TDtoObject, TDomainObject>(dto, respGet.Item);

            // Process After Map
            ApiUpdateAfterMapEvent<TDomainObject, TDtoObject> afterMapEvent = new ApiUpdateAfterMapEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respMapAfterRules = _businessRuleService.ExecuteEvent(afterMapEvent);
            response.CopyFrom(respMapAfterRules);
            if (!response.Success)
                return response;

            // Update item
            var respUpdate = _repository.Update(respGet.Item);
            response.CopyFrom(respUpdate);
            if (response.Error)
                return response;

            // Process After
            ApiUpdateAfterEvent<TDomainObject, TDtoObject> afterCommitEvent = new ApiUpdateAfterEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respAfterRules = _businessRuleService.ExecuteEvent(afterCommitEvent);
            response.CopyFrom(respAfterRules);
            if (!response.Success)
                return response;

            // Return item
            return Get(dto.StorageKey);
        }

        /// <summary>
        /// Update an item.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDtoObject>> UpdateAsync(TDtoObject dto)
        {
            var response = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (dto == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "dto"));
                return response;
            }

            // Process generic Dto object rules
            BusinessRuleContext bcontext = new BusinessRuleContext(dto);
            IResponse bizResp = await _businessRuleService.ExecuteRulesAsync(bcontext);
            response.CopyFrom(bizResp);
            if (!response.Success)
                return response;

            // GetItem
            var tempParam = _mapper.Map<TDtoObject, TDomainObject>(dto);
            var respGet = await _repository.GetAsync(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            if (respGet.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // Process Before
            ApiUpdateBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiUpdateBeforeEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respBeforeRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respBeforeRules);
            if (!response.Success)
                return response;

            // Map Dto to Domain
            _mapper.Map<TDtoObject, TDomainObject>(dto, respGet.Item);

            // Process After Map
            ApiUpdateAfterMapEvent<TDomainObject, TDtoObject> afterMapEvent = new ApiUpdateAfterMapEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respAfterMapRules = await _businessRuleService.ExecuteEventAsync(afterMapEvent);
            response.CopyFrom(respAfterMapRules);
            if (!response.Success)
                return response;

            // Update item
            var respUpdate = await _repository.UpdateAsync(respGet.Item);
            response.CopyFrom(respUpdate);
            if (response.Error)
                return response;

            // Process After
            ApiUpdateAfterEvent<TDomainObject, TDtoObject> afterCommitEvent = new ApiUpdateAfterEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respAfterRules = await _businessRuleService.ExecuteEventAsync(afterCommitEvent);
            response.CopyFrom(respAfterRules);
            if (!response.Success)
                return response;

            // Return item
            return await GetAsync(dto.StorageKey);
        }

        /// <summary>
        /// Create an item.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDtoObject> Create(TDtoObject dto)
        {
            var resp = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (dto == null)
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "dto"));
                return resp;
            }

            // Process generic Dto object rules
            BusinessRuleContext dtoContext = new BusinessRuleContext(dto);
            IResponse respRules = _businessRuleService.ExecuteRules(dtoContext);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            var newItem = new TDomainObject();

            // Process Before
            ApiCreateBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiCreateBeforeEvent<TDomainObject, TDtoObject>(newItem, dto);
            respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            // Map
            newItem = _mapper.Map<TDtoObject, TDomainObject>(dto);

            // Process After Map
            ApiCreateAfterMapEvent<TDomainObject, TDtoObject> afterMapEvent = new ApiCreateAfterMapEvent<TDomainObject, TDtoObject>(newItem, dto);
            var respAfterMapRules = _businessRuleService.ExecuteEvent(afterMapEvent);
            resp.CopyFrom(respAfterMapRules);
            if (!resp.Success)
                return resp;

            // Create
            var respCreate = _repository.Create(newItem);
            resp.CopyFrom(respCreate);
            if (resp.Error)
                return resp;

            // Process After
            ApiCreateAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiCreateAfterEvent<TDomainObject, TDtoObject>(newItem, dto);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            // GetItem
            var tempParam = _mapper.Map<TDomainObject, TDtoObject>(newItem);
            return Get(tempParam.StorageKey);
        }

        /// <summary>
        /// Create an item.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDtoObject>> CreateAsync(TDtoObject dto)
        {
            var resp = new ResponseItem<TDtoObject>();

            // Parameter validation
            if (dto == null)
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "dto"));
                return resp;
            }

            // Process generic Dto object rules
            BusinessRuleContext dtoContext = new BusinessRuleContext(dto);
            IResponse respRules = await _businessRuleService.ExecuteRulesAsync(dtoContext);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            var newItem = new TDomainObject();

            // Process Before
            ApiCreateBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiCreateBeforeEvent<TDomainObject, TDtoObject>(newItem, dto);
            respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            // Map
            newItem = _mapper.Map<TDtoObject, TDomainObject>(dto, newItem);

            // Process After Map
            ApiCreateAfterMapEvent<TDomainObject, TDtoObject> afterMapEvent = new ApiCreateAfterMapEvent<TDomainObject, TDtoObject>(newItem, dto);
            var respAfterMapRules = await _businessRuleService.ExecuteEventAsync(afterMapEvent);
            resp.CopyFrom(respAfterMapRules);
            if (!resp.Success)
                return resp;

            // Create
            var respCreate = await _repository.CreateAsync(newItem);
            resp.CopyFrom(respCreate);
            if (resp.Error)
                return resp;

            // Process After
            ApiCreateAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiCreateAfterEvent<TDomainObject, TDtoObject>(newItem, dto);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            resp.CopyFrom(respRules);
            if (!resp.Success)
                return resp;

            // GetItem
            var tempParam = _mapper.Map<TDomainObject, TDtoObject>(newItem);
            return await GetAsync(tempParam.StorageKey);
        }

        /// <summary>
        /// Delete an item.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual IResponse Delete(string storageKey)
        {
            var response = new Response();

            // Parameter validation
            if (string.IsNullOrEmpty(storageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "storageKey"));
                return response;
            }

            // GetItem
            var dto = new TDtoObject()
            {
                StorageKey = storageKey,
            };
            var tempParam = _mapper.Map<TDomainObject>(dto);
            var respGet = _repository.Get(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            if (respGet.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // Map to a DTO
            dto = _mapper.Map<TDomainObject, TDtoObject>(respGet.Item);

            // Process Before
            ApiDeleteBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiDeleteBeforeEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Delete
            var respDelete = _repository.Delete(respGet.Item);
            response.CopyFrom(respDelete);
            if (response.Error)
                return response;

            // Process After
            ApiDeleteAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiDeleteAfterEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            respRules = _businessRuleService.ExecuteEvent(afterEvent);
            response.CopyFrom(respRules);
            return response;
        }

        /// <summary>
        /// Delete an item.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> DeleteAsync(string storageKey)
        {
            var response = new Response();

            // Parameter validation
            if (string.IsNullOrEmpty(storageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "storageKey"));
                return response;
            }

            // GetItem
            var dto = new TDtoObject()
            {
                StorageKey = storageKey,
            };
            var tempParam = _mapper.Map<TDomainObject>(dto);
            var respGet = await _repository.GetAsync(tempParam);
            response.CopyFrom(respGet);
            if (response.Error)
                return response;

            if (respGet.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // Map to a DTO
            dto = _mapper.Map<TDomainObject, TDtoObject>(respGet.Item);

            // Process Before
            ApiDeleteBeforeEvent<TDomainObject, TDtoObject> beforeEvent = new ApiDeleteBeforeEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Delete
            var respDelete = await _repository.DeleteAsync(respGet.Item);
            response.CopyFrom(respDelete);
            if (response.Error)
                return response;

            // Process After
            ApiDeleteAfterEvent<TDomainObject, TDtoObject> afterEvent = new ApiDeleteAfterEvent<TDomainObject, TDtoObject>(respGet.Item, dto);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            return response;
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<ServiceQueryResponse<TDtoObject>> Query(ServiceQueryRequest request)
        {
            var response = new ResponseItem<ServiceQueryResponse<TDtoObject>>();

            // Process Before
            ApiQueryBeforeEvent<TDtoObject> beforeEvent = new ApiQueryBeforeEvent<TDtoObject>(request);
            var respRules = _businessRuleService.ExecuteEvent(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Query
            var result = _repository.Query(request);
            response.CopyFrom(result);
            if (response.Error)
                return response;

            response.Item = new ServiceQueryResponse<TDtoObject>();
            if (result.Item != null)
            {
                response.Item.Aggregate = result.Item.Aggregate;
                response.Item.Count = result.Item.Count;
                response.Item.List = _mapper.Map<List<TDomainObject>, List<TDtoObject>>(result.Item.List ?? new List<TDomainObject>());
            }

            // Process After
            ApiQueryAfterEvent<TDtoObject> afterEvent = new ApiQueryAfterEvent<TDtoObject>(request, response.Item);
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
        public virtual async Task<IResponseItem<ServiceQueryResponse<TDtoObject>>> QueryAsync(ServiceQueryRequest request)
        {
            var response = new ResponseItem<ServiceQueryResponse<TDtoObject>>();

            // Process Before
            ApiQueryBeforeEvent<TDtoObject> beforeEvent = new ApiQueryBeforeEvent<TDtoObject>(request);
            var respRules = await _businessRuleService.ExecuteEventAsync(beforeEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            // Query
            var result = await _repository.QueryAsync(request);
            response.CopyFrom(result);
            if (response.Error)
                return response;

            response.Item = new ServiceQueryResponse<TDtoObject>();
            if (result.Item != null)
            {
                response.Item.Aggregate = result.Item.Aggregate;
                response.Item.Count = result.Item.Count;
                response.Item.List = _mapper.Map<List<TDomainObject>, List<TDtoObject>>(result.Item.List ?? new List<TDomainObject>());
            }

            // Process After
            ApiQueryAfterEvent<TDtoObject> afterEvent = new ApiQueryAfterEvent<TDtoObject>(request, response.Item);
            respRules = await _businessRuleService.ExecuteEventAsync(afterEvent);
            response.CopyFrom(respRules);
            if (!response.Success)
                return response;

            return response;
        }
    }
}