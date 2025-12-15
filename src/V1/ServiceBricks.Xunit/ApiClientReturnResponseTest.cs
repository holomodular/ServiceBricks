using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public abstract class ApiClientReturnResponseTest<TDto>
        where TDto : class, IDataTransferObject, new()
    {
        public virtual ISystemManager SystemManager { get; set; }

        public virtual ITestManager<TDto> TestManager { get; set; }

        protected virtual ServiceQueryRequest GetDefaultServiceQueryRequest()
        {
            return new ServiceQueryRequestBuilder()
                .Paging(1, int.MaxValue, true)
                .Build();
        }

        #region Async

        [Fact]
        public virtual async Task Create_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Create
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreate = await client.CreateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreate.Error);
        }

        public virtual async Task<TDto> CreateBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            //Call Create
            var respCreate = await client.CreateAsync(model);

            //Validate
            TestManager.ValidateObjects(model, respCreate.Item, HttpMethod.Post);

            //Call GetItem
            var respGetItem = await client.GetAsync(respCreate.Item.StorageKey);

            //Validate
            TestManager.ValidateObjects(model, respGetItem.Item, HttpMethod.Post);

            //Call GetAll
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, respGetItem.Item);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(respGetItem.Item, foundObject, HttpMethod.Get);
            return respGetItem.Item;
        }

        [Fact]
        public virtual async Task Create_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Create_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Create_TwoReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;

            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = await CreateBaseReturnResponseAsync(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = await CreateBaseReturnResponseAsync(maxmodel);

            //Call GetAll again after create
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Item.List.Count == 2 + existingCount);

            // Cleanup
            await DeleteBaseReturnResponseAsync(mindto);
            await DeleteBaseReturnResponseAsync(maxdto);
        }


        [Fact]
        public virtual async Task CreateAck_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call CreateAck
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreateAck = await client.CreateAckAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreateAck.Error);
        }

        public virtual async Task<TDto> CreateAckBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            //Call CreateAck
            var respCreateAck = await client.CreateAckAsync(model);

            Assert.True(respCreateAck.Success);

            //Call GetAll and find recently created
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            //Find new one
            TDto found = null;
            foreach (var item in respGetAll.Item.List)
            {
                bool existingFound = false;
                foreach (var existingitem in existingList)
                {
                    if (existingitem.StorageKey == item.StorageKey)
                    {
                        existingFound = true;
                        break;
                    }
                }
                if (existingFound)
                    continue;
                found = item; //do this for cleanup
            }
            Assert.True(found != null);

            //Validate
            TestManager.ValidateObjects(model, found, HttpMethod.Post);

            //Call GetItem
            var respGetItem = await client.GetAsync(found.StorageKey);

            //Validate
            TestManager.ValidateObjects(model, respGetItem.Item, HttpMethod.Post);

            //Call GetAll
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, respGetItem.Item);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(respGetItem.Item, foundObject, HttpMethod.Get);
            return respGetItem.Item;
        }

        [Fact]
        public virtual async Task CreateAck_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateAckBaseReturnResponseAsync(model);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task CreateAck_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateAckBaseReturnResponseAsync(model);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }


        [Fact]
        public virtual async Task GetAll_MinDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task GetAll_MaxDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task GetItem_NullStringReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = await client.GetAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetItem_EmptyStringReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = await client.GetAsync(string.Empty);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetItem_NotFoundReturnResponseAsync()
        {
            var model = TestManager.GetObjectNotFound();

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = await client.GetAsync(model.StorageKey);
            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);

            //Call GetItem with null
            respGetItem = await client.GetAsync(null);

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetAllPaging_MultiReturnResponseAsync()
        {
            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = await CreateBaseReturnResponseAsync(maxmodel);

            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = await CreateBaseReturnResponseAsync(minmodel);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            Assert.True(existingCount >= 2); //Possible pre-loaded data

            //Call GetAllPaging get one
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            var respPaging = await client.QueryAsync(serviceQueryRequest);

            Assert.True(respPaging.Success);
            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 1);

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respPaging = await client.QueryAsync(serviceQueryRequest);

            Assert.True(respPaging.Success);
            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 2);

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respPaging = await client.QueryAsync(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == existingCount);

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respPaging = await client.QueryAsync(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 1);

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respPaging = await client.QueryAsync(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 0);

            // Cleanup
            await DeleteBaseReturnResponseAsync(mindto);
            await DeleteBaseReturnResponseAsync(maxdto);
        }

        [Fact]
        public virtual async Task Update_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdate = await client.UpdateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdate.Error);
        }

        protected virtual async Task UpdateNoChangeBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual async Task Update_NoChange_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        protected virtual async Task UpdateBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = await client.UpdateAsync(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual async Task Update_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }



        [Fact]
        public virtual async Task UpdateAck_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdateAck = await client.UpdateAckAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdateAck.Error);
        }

        protected virtual async Task UpdateAckNoChangeBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respUpdateAck = await client.UpdateAckAsync(model);

            Assert.True(respUpdateAck.Success);
            
        }

        [Fact]
        public virtual async Task UpdateAck_NoChange_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateAckNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task UpdateAck_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateAckNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        protected virtual async Task UpdateAckBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //UpdateAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call UpdateAck
            var respUpdateAck = await client.UpdateAckAsync(model);

            Assert.True(respUpdateAck.Success);

        }

        [Fact]
        public virtual async Task UpdateAck_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateAckBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task UpdateAck_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateAckBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }


        protected virtual async Task DeleteBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = await client.DeleteAsync(model.StorageKey);

            Assert.True(respDelete.Success);

            //Verify GetItem not found
            var respGetItem = await client.GetAsync(model.StorageKey);

            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);
        }

        [Fact]
        public virtual async Task Delete_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Delete_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task QueryReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            // Query all
            var respQueryAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respQueryAll.Success);
            Assert.True(existingCount == respQueryAll.Item.Count);
            Assert.True(existingCount == respQueryAll.Item.List.Count);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            // GetAll and find object
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Query all again and find
            respQueryAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respQueryAll.Success);
            Assert.True(respQueryAll.Item.Count == 1 + existingCount);
            Assert.True(respQueryAll.Item.List.Count == 1 + existingCount);

            foundObject = TestManager.FindObject(respQueryAll.Item.List, dto);
            Assert.True(foundObject != null);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Query_ByPropertiesReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQuery = await client.QueryAsync(query);

                Assert.True(respQuery.Item.List.Count >= 1);
                var foundObject = TestManager.FindObject(respQuery.Item.List, dto);
                Assert.True(foundObject != null);
            }

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Patch_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatch = await client.PatchAsync(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatch.Error);
        }

        protected virtual async Task PatchNoChangeBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respPatch = await client.PatchAsync(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatch.Success);

        }

        [Fact]
        public virtual async Task Patch_NoChange_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await PatchNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Patch_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await PatchNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        protected virtual async Task PatchBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //Patch the object
            if (model == null)
                throw new ArgumentNullException("model");

            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var copy = mapper.Map<TDto, TDto>(model);
            TestManager.UpdateObject(model);

            var patch = JsonPatchHelper.CreatePatch(copy, model);

            //Call Patch
            var respPatch = await client.PatchAsync(model.StorageKey, patch);
            Assert.True(respPatch.Success);

            var respGet = await client.GetAsync(model.StorageKey);
            TestManager.ValidateObjects(model, respGet.Item, HttpMethod.Patch);
        }

        [Fact]
        public virtual async Task Patch_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await PatchBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Patch_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await PatchBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }


        [Fact]
        public virtual async Task PatchAck_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatchAck = await client.PatchAckAsync(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatchAck.Error);
        }

        protected virtual async Task PatchAckNoChangeBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respPatchAck = await client.PatchAckAsync(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatchAck.Success);
        }

        [Fact]
        public virtual async Task PatchAck_NoChange_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await PatchAckNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task PatchAck_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await PatchAckNoChangeBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        protected virtual async Task PatchAckBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //PatchAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var copy = mapper.Map<TDto, TDto>(model);

            TestManager.UpdateObject(model);

            var patch = JsonPatchHelper.CreatePatch(copy, model);

            //Call PatchAck
            var respPatchAck = await client.PatchAckAsync(model.StorageKey, patch);

            Assert.True(respPatchAck.Success);

        }

        [Fact]
        public virtual async Task PatchAck_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await PatchAckBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task PatchAck_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await PatchAckBaseReturnResponseAsync(dto);

            // Cleanup
            await DeleteBaseReturnResponseAsync(dto);
        }


        [Fact]
        public virtual async Task Validate_NullDataReturnResponseAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Validate
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respValidate = await client.ValidateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respValidate.Error);
        }

        public virtual async Task ValidateBaseReturnResponseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            
            //Call Validate
            var respValidate = await client.ValidateAsync(model);

            Assert.True(respValidate.Success);
        }

        [Fact]
        public virtual async Task Validate_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            await ValidateBaseReturnResponseAsync(model);            
        }

        [Fact]
        public virtual async Task Validate_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            await ValidateBaseReturnResponseAsync(model);

        }


        #endregion Async

        #region Sync

        [Fact]
        public virtual void Create_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Create
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreate = client.Create(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreate.Error);
        }

        public virtual TDto CreateBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            //Call Create
            var respCreate = client.Create(model);

            //Validate
            TestManager.ValidateObjects(model, respCreate.Item, HttpMethod.Post);

            //Call GetItem
            var respGetItem = client.Get(respCreate.Item.StorageKey);

            //Validate
            TestManager.ValidateObjects(model, respGetItem.Item, HttpMethod.Post);

            //Call GetAll
            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, respGetItem.Item);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(respGetItem.Item, foundObject, HttpMethod.Get);
            return respGetItem.Item;
        }

        [Fact]
        public virtual void Create_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Create_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Create_TwoReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;

            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = CreateBaseReturnResponse(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = CreateBaseReturnResponse(maxmodel);

            //Call GetAll again after create
            respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Item.List.Count == 2 + existingCount);

            // Cleanup
            DeleteBaseReturnResponse(mindto);
            DeleteBaseReturnResponse(maxdto);
        }

        [Fact]
        public virtual void CreateAck_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call CreateAck
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreateAck = client.CreateAck(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreateAck.Error);
        }

        public virtual TDto CreateAckBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            //Call CreateAck
            var respCreateAck = client.CreateAck(model);

            //Call GetAll and find recently created
            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            //Find new one
            TDto found = null;
            foreach (var item in respGetAll.Item.List)
            {
                bool existingFound = false;
                foreach (var existingitem in existingList)
                {
                    if (existingitem.StorageKey == item.StorageKey)
                    {
                        existingFound = true;
                        break;
                    }
                }
                if (existingFound)
                    continue;
                found = item; //do this for cleanup
            }
            Assert.True(found != null);

            //Validate
            TestManager.ValidateObjects(model, found, HttpMethod.Post);

            //Call GetItem
            var respGetItem = client.Get(found.StorageKey);

            //Validate
            TestManager.ValidateObjects(model, respGetItem.Item, HttpMethod.Post);

            //Call GetAll
            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, respGetItem.Item);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(respGetItem.Item, foundObject, HttpMethod.Get);
            return respGetItem.Item;
        }

        [Fact]
        public virtual void CreateAck_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateAckBaseReturnResponse(model);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void CreateAck_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateAckBaseReturnResponse(model);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }



        [Fact]
        public virtual void GetAll_MinDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);

            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void GetAll_MaxData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void GetItem_NullStringReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = client.Get(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetItem_EmptyStringReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = client.Get(string.Empty);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetItem_NotFoundReturnResponse()
        {
            var model = TestManager.GetObjectNotFound();

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = client.Get(model.StorageKey);
            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);

            //Call GetItem with null
            respGetItem = client.Get(null);

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetAllPaging_MultiReturnResponse()
        {
            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = CreateBaseReturnResponse(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = CreateBaseReturnResponse(maxmodel);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            Assert.True(existingCount >= 2); //Possible pre-loaded data

            //Call GetAllPaging get one
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            var respPaging = client.Query(serviceQueryRequest);

            Assert.True(respPaging.Success);
            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 1);

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respPaging = client.Query(serviceQueryRequest);

            Assert.True(respPaging.Success);
            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 2);

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respPaging = client.Query(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == existingCount);

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respPaging = client.Query(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 1);

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respPaging = client.Query(serviceQueryRequest);

            Assert.True(respPaging.Item.List != null);
            if (respPaging.Item.List == null)
                throw new ArgumentNullException(nameof(respPaging.Item.List));
            Assert.True(respPaging.Success == true);
            Assert.True(respPaging.Error == false);
            Assert.True(respPaging.Item.Count == existingCount);
            Assert.True(respPaging.Item.List.Count == 0);

            // Cleanup
            DeleteBaseReturnResponse(mindto);
            DeleteBaseReturnResponse(maxdto);
        }

        [Fact]
        public virtual void Update_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdate = client.Update(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdate.Error);
        }

        protected virtual void UpdateNoChangeBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = client.Update(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual void Update_NoChange_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Update_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        protected virtual void UpdateBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = client.Update(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual void Update_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Update_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }


        [Fact]
        public virtual void UpdateAck_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdateAck = client.UpdateAck(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdateAck.Error);
        }

        protected virtual void UpdateAckNoChangeBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respUpdateAck = client.UpdateAck(model);

            Assert.True(respUpdateAck.Success);
        }

        [Fact]
        public virtual void UpdateAck_NoChange_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateAckNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void UpdateAck_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateAckNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        protected virtual void UpdateAckBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //UpdateAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call UpdateAck
            var respUpdateAck = client.UpdateAck(model);

            Assert.True(respUpdateAck.Success);
            
        }

        [Fact]
        public virtual void UpdateAck_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateAckBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void UpdateAck_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateAckBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        protected virtual void DeleteBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = client.Delete(model.StorageKey);

            Assert.True(respDelete.Success);

            //Verify GetItem not found
            var respGetItem = client.Get(model.StorageKey);

            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);
        }

        [Fact]
        public virtual void Delete_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);

            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Delete_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void QueryReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            // Query all
            var respQueryAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respQueryAll.Success);
            Assert.True(existingCount == respQueryAll.Item.Count);
            Assert.True(existingCount == respQueryAll.Item.List.Count);

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            // GetAll and find object
            respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Query all again and find
            respQueryAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respQueryAll.Success);
            Assert.True(respQueryAll.Item.Count == 1 + existingCount);
            Assert.True(respQueryAll.Item.List.Count == 1 + existingCount);

            foundObject = TestManager.FindObject(respQueryAll.Item.List, dto);
            Assert.True(foundObject != null);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Query_ByPropertiesReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQuery = client.Query(query);

                Assert.True(respQuery.Item.List.Count >= 1);
                var foundObject = TestManager.FindObject(respQuery.Item.List, dto);
                Assert.True(foundObject != null);
            }

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Patch_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatch = client.Patch(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatch.Error);
        }

        protected virtual void PatchNoChangeBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respPatch = client.Patch(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatch.Success);
            
        }

        [Fact]
        public virtual void Patch_NoChange_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            PatchNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Patch_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            PatchNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        protected virtual void PatchBaseReturnResponse(TDto model) 
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //Patch the object
            if (model == null)
                throw new ArgumentNullException("model");

            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var copy = mapper.Map<TDto, TDto>(model);
            TestManager.UpdateObject(model);

            var patch = JsonPatchHelper.CreatePatch(copy, model);

            //Call Patch
            var respPatch = client.Patch(model.StorageKey, patch);
            Assert.True(respPatch.Success);

            var respGet  = client.Get(model.StorageKey);
            TestManager.ValidateObjects(model, respGet.Item, HttpMethod.Patch);
        }

        [Fact]
        public virtual void Patch_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);
            PatchBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Patch_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);
            PatchBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }


        [Fact]
        public virtual void PatchAck_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatchAck = client.PatchAck(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatchAck.Error);
        }

        protected virtual void PatchAckNoChangeBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            var respPatchAck = client.PatchAck(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatchAck.Success);
        }

        [Fact]
        public virtual void PatchAck_NoChange_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            PatchAckNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void PatchAck_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            PatchAckNoChangeBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        protected virtual void PatchAckBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);
            //PatchAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var copy = mapper.Map<TDto, TDto>(model);            

            TestManager.UpdateObject(model);

            var patch = JsonPatchHelper.CreatePatch(copy, model);

            //Call PatchAck
            var respPatchAck = client.PatchAck(model.StorageKey, patch);

            Assert.True(respPatchAck.Success);

        }

        [Fact]
        public virtual void PatchAck_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);
            PatchAckBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void PatchAck_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);
            PatchAckBaseReturnResponse(dto);

            // Cleanup
            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Validate_NullDataReturnResponse()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Validate
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respValidate = client.Validate(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respValidate.Error);
        }

        public virtual void ValidateBaseReturnResponse(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClientReturnResponse(SystemManager.ServiceProvider);

            //Call Validate
            var respValidate = client.Validate(model);


            Assert.True(respValidate.Success);
        }

        [Fact]
        public virtual void Validate_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            ValidateBaseReturnResponse(model);
        }

        [Fact]
        public virtual void Validate_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();

            ValidateBaseReturnResponse(model);

        }

        #endregion Sync
    }
}