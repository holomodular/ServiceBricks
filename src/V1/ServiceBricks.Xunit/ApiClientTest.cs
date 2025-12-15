using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public abstract class ApiClientTest<TDto>
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
        public virtual async Task Create_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Create
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreate = await client.CreateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreate.Error);
        }

        public virtual async Task<TDto> CreateBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual async Task Create_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Create_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Create_TwoAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;

            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = await CreateBaseAsync(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = await CreateBaseAsync(maxmodel);

            //Call GetAll again after create
            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Item.List.Count == 2 + existingCount);

            // Cleanup
            await DeleteBaseAsync(mindto);
            await DeleteBaseAsync(maxdto);
        }


        [Fact]
        public virtual async Task CreateAck_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call CreateAck
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreateAck = await client.CreateAckAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreateAck.Error);
        }

        public virtual async Task<TDto> CreateAckBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            //Call CreateAck
            var respCreateAck = await client.CreateAckAsync(model);

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
        public virtual async Task CreateAck_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateAckBaseAsync(model);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task CreateAck_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateAckBaseAsync(model);

            // Cleanup
            await DeleteBaseAsync(dto);
        }


        [Fact]
        public virtual async Task GetAll_MinDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);

            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task GetAll_MaxDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

            respGetAll = await client.QueryAsync(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task GetItem_NullStringAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = await client.GetAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetItem_EmptyStringAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = await client.GetAsync(string.Empty);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetItem_NotFoundAsync()
        {
            var model = TestManager.GetObjectNotFound();

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetItem = await client.GetAsync(model.StorageKey);
            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);

            //Call GetItem with null
            respGetItem = await client.GetAsync(null);

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual async Task GetAllPaging_MultiAsync()
        {
            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = await CreateBaseAsync(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = await CreateBaseAsync(maxmodel);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
            await DeleteBaseAsync(mindto);
            await DeleteBaseAsync(maxdto);
        }

        [Fact]
        public virtual async Task Update_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdate = await client.UpdateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdate.Error);
        }

        protected virtual async Task UpdateNoChangeBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual async Task Update_NoChange_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        protected virtual async Task UpdateBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual async Task Update_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }


        [Fact]
        public virtual async Task UpdateAck_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdateAck = await client.UpdateAckAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdateAck.Error);
        }

        protected virtual async Task UpdateAckNoChangeBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respUpdateAck = await client.UpdateAckAsync(model);

            Assert.True(respUpdateAck.Success);

        }

        [Fact]
        public virtual async Task UpdateAck_NoChange_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateAckNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task UpdateAck_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateAckNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        protected virtual async Task UpdateAckBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            //UpdateAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call UpdateAck
            var respUpdateAck = await client.UpdateAckAsync(model);

            Assert.True(respUpdateAck.Success);

            // Get the item
            var respUpdated = await client.GetAsync(model.StorageKey);

            //Validate
            TestManager.ValidateObjects(model, respUpdated.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual async Task UpdateAck_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateAckBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task UpdateAck_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateAckBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }


        protected virtual async Task DeleteBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = await client.DeleteAsync(model.StorageKey);

            Assert.True(respDelete.Success);

            //Verify GetItem not found
            var respGetItem = await client.GetAsync(model.StorageKey);

            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);
        }

        [Fact]
        public virtual async Task Delete_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);

            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Delete_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task QueryAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
            var dto = await CreateBaseAsync(model);

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
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Query_ByPropertiesAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

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
            await DeleteBaseAsync(dto);
        }


        [Fact]
        public virtual async Task Patch_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatch = await client.PatchAsync(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatch.Error);
        }

        protected virtual async Task PatchNoChangeBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respPatch = await client.PatchAsync(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatch.Success);

        }

        [Fact]
        public virtual async Task Patch_NoChange_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            await PatchNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Patch_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            await PatchNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        protected virtual async Task PatchBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual async Task Patch_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);
            await PatchBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Patch_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);
            await PatchBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }


        [Fact]
        public virtual async Task PatchAck_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatchAck = await client.PatchAckAsync(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatchAck.Error);
        }

        protected virtual async Task PatchAckNoChangeBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respPatchAck = await client.PatchAckAsync(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatchAck.Success);
        }

        [Fact]
        public virtual async Task PatchAck_NoChange_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            await PatchAckNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task PatchAck_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            await PatchAckNoChangeBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        protected virtual async Task PatchAckBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual async Task PatchAck_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);
            await PatchAckBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task PatchAck_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);
            await PatchAckBaseAsync(dto);

            // Cleanup
            await DeleteBaseAsync(dto);
        }


        [Fact]
        public virtual async Task Validate_NullDataAsync()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Validate
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respValidate = await client.ValidateAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respValidate.Error);
        }

        public virtual async Task ValidateBaseAsync(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

           
            var client = TestManager.GetClient(SystemManager.ServiceProvider);            

            //Call Validate
            var respValidate = await client.ValidateAsync(model);

            Assert.True(respValidate.Success);
        }

        [Fact]
        public virtual async Task Validate_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            await ValidateBaseAsync(model);
            
        }

        [Fact]
        public virtual async Task Validate_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            await ValidateBaseAsync(model);
        }



        #endregion Async

        #region Sync

        [Fact]
        public virtual void Create_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Create
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreate = client.Create(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreate.Error);
        }

        public virtual TDto CreateBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual void Create_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Create_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Create_Two()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;

            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = CreateBase(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = CreateBase(maxmodel);

            //Call GetAll again after create
            respGetAll = client.Query(GetDefaultServiceQueryRequest());
            Assert.True(respGetAll.Item.List.Count == 2 + existingCount);

            // Cleanup
            DeleteBase(mindto);
            DeleteBase(maxdto);
        }


        [Fact]
        public virtual void CreateAck_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call CreateAck
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respCreateAck = client.CreateAck(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respCreateAck.Error);
        }

        public virtual TDto CreateAckBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual void CreateAck_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateAckBase(model);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void CreateAck_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateAckBase(model);

            // Cleanup
            DeleteBase(dto);
        }


        [Fact]
        public virtual void GetAll_MinData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);

            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void GetAll_MaxData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Success);
            existingCount = respGetAll.Item.List.Count;
            existingList = respGetAll.Item.List;

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

            respGetAll = client.Query(GetDefaultServiceQueryRequest());

            Assert.True(respGetAll.Item.List.Count == 1 + existingCount);
            var foundObject = TestManager.FindObject(respGetAll.Item.List, dto);
            Assert.True(foundObject != null);

            //Validate
            TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void GetItem_NullString()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = client.Get(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetItem_EmptyString()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respGetItem = client.Get(string.Empty);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetItem_NotFound()
        {
            var model = TestManager.GetObjectNotFound();

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call GetItem
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respGetItem = client.Get(model.StorageKey);
            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);

            //Call GetItem with null
            respGetItem = client.Get(null);

            Assert.True(respGetItem.Error);
        }

        [Fact]
        public virtual void GetAllPaging_Multi()
        {
            var minmodel = TestManager.GetMinimumDataObject();
            var mindto = CreateBase(minmodel);

            var maxmodel = TestManager.GetMaximumDataObject();
            var maxdto = CreateBase(maxmodel);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
            DeleteBase(mindto);
            DeleteBase(maxdto);
        }

        [Fact]
        public virtual void Update_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdate = client.Update(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdate.Error);
        }

        protected virtual void UpdateNoChangeBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Update
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respUpdate = client.Update(model);

            Assert.True(respUpdate.Success);

            //Validate
            TestManager.ValidateObjects(model, respUpdate.Item, HttpMethod.Put);
        }

        [Fact]
        public virtual void Update_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            UpdateNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Update_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            UpdateNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        protected virtual void UpdateBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual void Update_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);
            UpdateBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Update_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);
            UpdateBase(dto);

            // Cleanup
            DeleteBase(dto);
        }


        [Fact]
        public virtual void UpdateAck_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respUpdateAck = client.UpdateAck(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respUpdateAck.Error);
        }

        protected virtual void UpdateAckNoChangeBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call UpdateAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respUpdateAck = client.UpdateAck(model);

            Assert.True(respUpdateAck.Success);

        }

        [Fact]
        public virtual void UpdateAck_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            UpdateAckNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void UpdateAck_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            UpdateAckNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        protected virtual void UpdateAckBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            //UpdateAck the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call UpdateAck
            var respUpdateAck = client.UpdateAck(model);

            Assert.True(respUpdateAck.Success);

        }

        [Fact]
        public virtual void UpdateAck_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);
            UpdateAckBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void UpdateAck_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);
            UpdateAckBase(dto);

            // Cleanup
            DeleteBase(dto);
        }


        protected virtual void DeleteBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = client.Delete(model.StorageKey);

            Assert.True(respDelete.Success);

            //Verify GetItem not found
            var respGetItem = client.Get(model.StorageKey);

            Assert.True(respGetItem.Success);
            Assert.True(respGetItem.Item == null);
        }

        [Fact]
        public virtual void Delete_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);

            DeleteBase(dto);
        }

        [Fact]
        public virtual void Delete_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

            DeleteBase(dto);
        }

        [Fact]
        public virtual void Query()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
            var dto = CreateBase(model);

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
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Query_ByProperties()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

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
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Patch_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatch = client.Patch(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatch.Error);
        }

        protected virtual void PatchNoChangeBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call Patch
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respPatch = client.Patch(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatch.Success);

        }

        [Fact]
        public virtual void Patch_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            PatchNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Patch_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            PatchNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        protected virtual void PatchBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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

            var respGet = client.Get(model.StorageKey);
            TestManager.ValidateObjects(model, respGet.Item, HttpMethod.Patch);
        }

        [Fact]
        public virtual void Patch_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);
            PatchBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Patch_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);
            PatchBase(dto);

            // Cleanup
            DeleteBase(dto);
        }


        [Fact]
        public virtual void PatchAck_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respPatchAck = client.PatchAck(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respPatchAck.Error);
        }

        protected virtual void PatchAckNoChangeBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Call PatchAck
            var client = TestManager.GetClient(SystemManager.ServiceProvider);
            var respPatchAck = client.PatchAck(model.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>());

            Assert.True(respPatchAck.Success);
        }

        [Fact]
        public virtual void PatchAck_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            PatchAckNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void PatchAck_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            PatchAckNoChangeBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        protected virtual void PatchAckBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);
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
        public virtual void PatchAck_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);
            PatchAckBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void PatchAck_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);
            PatchAckBase(dto);

            // Cleanup
            DeleteBase(dto);
        }

        [Fact]
        public virtual void Validate_NullData()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Validate
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var respValidate = client.Validate(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.True(respValidate.Error);
        }

        public virtual void ValidateBase(TDto model)
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));
            
            var client = TestManager.GetClient(SystemManager.ServiceProvider);

            //Call Validate
            var respValidate = client.Validate(model);


            Assert.True(respValidate.Success);
        }

        [Fact]
        public virtual void Validate_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            ValidateBase(model);
        }

        [Fact]
        public virtual void Validate_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            ValidateBase(model);

        }


        #endregion Sync
    }
}