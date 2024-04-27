using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public abstract class ApiControllerTest<TDto>
        where TDto : class, IDataTransferObject
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
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Create
            var respCreate = await controller.CreateAsync(null);

            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Create_NullDataReturnResponseAsync()
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            //Call Create
            var respCreate = await controller.CreateAsync(null);

            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                {
                    Assert.True(resp.Error);
                    Assert.NotNull(resp.Messages);
                    Assert.True(resp.Messages.Count > 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        public virtual async Task<TDto> CreateBaseAsync(TDto model)
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();

            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultGetAll)
            {
                Assert.True(okResultGetAll.Value != null);
                if (okResultGetAll.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
            }
            else
                Assert.Fail("");

            //Call Create
            var respCreate = await controller.CreateAsync(model);
            if (respCreate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Post);

                    //Call GetItem
                    var respGetItem = await controller.GetAsync(obj.StorageKey);
                    if (respGetItem is OkObjectResult okResultGetItem && model != null)
                    {
                        Assert.True(okResultGetItem.Value != null);
                        if (okResultGetItem.Value is TDto gobj)
                        {
                            //Validate
                            TestManager.ValidateObjects(model, gobj, HttpMethod.Post);

                            //Call GetAll
                            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
                            if (respGetAll is OkObjectResult okResultGetAll2)
                            {
                                Assert.True(okResultGetAll2.Value != null);
                                if (okResultGetAll2.Value is ServiceQueryResponse<TDto> sqr)
                                {
                                    Assert.True(sqr.List.Count == 1 + existingCount);
                                    var foundObject = TestManager.FindObject(sqr.List, gobj);
                                    Assert.True(foundObject != null);

                                    //Validate
                                    TestManager.ValidateObjects(gobj, foundObject, HttpMethod.Get);
                                    return gobj;
                                }
                                else
                                    Assert.Fail("");
                            }
                            else
                                Assert.Fail("");
                        }
                        else
                            Assert.Fail("");
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            throw new Exception();
        }

        public virtual async Task<TDto> CreateBaseReturnResponseAsync(TDto model)
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();

            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultGetAll)
            {
                Assert.True(okResultGetAll.Value != null);
                if (okResultGetAll.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
            }
            else
                Assert.Fail("");

            //Call Create
            var respCreate = await controller.CreateAsync(model);
            if (respCreate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Post);

                    //Call GetItem
                    var respGetItem = await controller.GetAsync(resp.Item.StorageKey);
                    if (respGetItem is OkObjectResult okResultGetItem && model != null)
                    {
                        Assert.True(okResultGetItem.Value != null);
                        if (okResultGetItem.Value is ResponseItem<TDto> gobj)
                        {
                            //Validate
                            TestManager.ValidateObjects(model, gobj.Item, HttpMethod.Post);

                            //Call GetAll
                            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
                            if (respGetAll is OkObjectResult okResultGetAll2)
                            {
                                Assert.True(okResultGetAll2.Value != null);
                                if (okResultGetAll2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                                {
                                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                                    var foundObject = TestManager.FindObject(sqr.Item.List, gobj.Item);
                                    Assert.True(foundObject != null);

                                    //Validate
                                    TestManager.ValidateObjects(gobj.Item, foundObject, HttpMethod.Get);
                                    return gobj.Item;
                                }
                                else
                                    Assert.Fail("");
                            }
                            else
                                Assert.Fail("");
                        }
                        else
                            Assert.Fail("");
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            throw new Exception();
        }

        [Fact]
        public virtual async Task Create_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            await CreateBaseAsync(model);
        }

        [Fact]
        public virtual async Task Create_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            await CreateBaseReturnResponseAsync(model);
        }

        [Fact]
        public virtual async Task Create_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            await CreateBaseAsync(model);
        }

        [Fact]
        public virtual async Task Create_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            await CreateBaseReturnResponseAsync(model);
        }

        [Fact]
        public virtual async Task Create_TwoAsync()
        {
            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            await Create_MinDataAsync();

            await Create_MaxDataAsync();

            //Call GetAll again after create
            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 2 + existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Create_TwoReturnResponseAsync()
        {
            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            await Create_MinDataReturnResponseAsync();

            await Create_MaxDataReturnResponseAsync();

            //Call GetAll again after create
            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 2 + existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MinDataAsync()
        {
            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MinDataReturnResponseAsync()
        {
            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MaxDataAsync()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MaxDataReturnResponseAsync()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NullDataAsync()
        {
            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = await controller.GetAsync(null);
            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NullDataReturnResponseAsync()
        {
            //Call GetItem
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = await controller.GetAsync(null);
            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NotFoundAsync()
        {
            var model = TestManager.GetObjectNotFound();

            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = await controller.GetAsync(model.StorageKey);

            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");

            //Call GetItem with null
            respGetItem = await controller.GetAsync(null);

            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NotFoundReturnResponseAsync()
        {
            var model = TestManager.GetObjectNotFound();

            //Call GetItem
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = await controller.GetAsync(model.StorageKey);

            if (respGetItem is OkObjectResult okResult)
            {
                if (okResult.Value is Response resp)
                    Assert.True(resp.Success);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetItem with null
            respGetItem = await controller.GetAsync(null);

            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAllPaging_MultiAsync()
        {
            await Create_TwoAsync();

            int existingCount = 0;
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultAll)
            {
                Assert.True(okResultAll.Value != null);
                if (okResultAll.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    Assert.True(existingCount >= 2); //Possible pre-loaded data
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get one
            var paging = new Paging() { PageNumber = 1, PageSize = 1 };
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == 2);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult3)
            {
                Assert.True(okResult3.Value != null);
                if (okResult3.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2)
            {
                Assert.True(okResultpage2.Value != null);
                if (okResultpage2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2ofmax)
            {
                Assert.True(okResultpage2ofmax.Value != null);
                if (okResultpage2ofmax.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAllPaging_MultiReturnResponseAsync()
        {
            await Create_TwoReturnResponseAsync();

            int existingCount = 0;
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultAll)
            {
                Assert.True(okResultAll.Value != null);
                if (okResultAll.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    Assert.True(existingCount >= 2); //Possible pre-loaded data
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get one
            var paging = new Paging() { PageNumber = 1, PageSize = 1 };
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));
                    Assert.True(respPaging.Item.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 2);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult3)
            {
                Assert.True(okResult3.Value != null);
                if (okResult3.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2)
            {
                Assert.True(okResultpage2.Value != null);
                if (okResultpage2.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respGetAll = await controller.QueryAsync(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2ofmax)
            {
                Assert.True(okResultpage2ofmax.Value != null);
                if (okResultpage2ofmax.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_NullDataAsync()
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.UpdateAsync(null);

            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_NullDataReturnResponseAsync()
        {
            //Call Update
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = await controller.UpdateAsync(null);

            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual async Task UpdateNoChangeBaseAsync(TDto model)
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.UpdateAsync(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual async Task UpdateNoChangeBaseReturnResponseAsync(TDto model)
        {
            //Call Update
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = await controller.UpdateAsync(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_NoChange_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateNoChangeBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateNoChangeBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseAsync(model);

            await UpdateNoChangeBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBaseReturnResponseAsync(model);

            await UpdateNoChangeBaseReturnResponseAsync(dto);
        }

        protected virtual async Task UpdateBaseAsync(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = await controller.UpdateAsync(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual async Task UpdateBaseReturnResponseAsync(TDto model)
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = await controller.UpdateAsync(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_MinDataReturnResponseAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_MaxDataAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);
            await UpdateBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Update_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);
            await UpdateBaseReturnResponseAsync(dto);
        }

        protected virtual async Task DeleteBaseAsync(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = await controller.DeleteAsync(model.StorageKey);
            if (respDelete is OkObjectResult okResultDelete)
            {
                Assert.True(okResultDelete.Value != null);
                if (okResultDelete.Value is bool retVal)
                {
                    Assert.True(retVal == true);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify GetItem not found
            var respGetItem = await controller.GetAsync(model.StorageKey);
            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");
        }

        protected virtual async Task DeleteBaseReturnResponseAsync(TDto model)
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = await controller.DeleteAsync(model.StorageKey);
            if (respDelete is OkObjectResult okResultDelete)
            {
                Assert.True(okResultDelete.Value != null);
                if (okResultDelete.Value is Response retVal)
                {
                    Assert.True(retVal.Success);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify GetItem not found
            var respGetItem = await controller.GetAsync(model.StorageKey);
            if (respGetItem is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    Assert.True(resp.Success);
                    Assert.True(resp.Item == null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Delete_MinDataAsync()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);

            await DeleteBaseAsync(dto);
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
            var dto = await CreateBaseAsync(model);

            await DeleteBaseAsync(dto);
        }

        [Fact]
        public virtual async Task Delete_MaxDataReturnResponseAsync()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            await DeleteBaseReturnResponseAsync(dto);
        }

        [Fact]
        public virtual async Task QueryAsync()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all
            var respQueryAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qokResult)
            {
                Assert.True(qokResult.Value != null);
                if (qokResult.Value is ServiceQueryResponse<TDto> querycount)
                {
                    Assert.True(existingCount == querycount.List.Count);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all again
            respQueryAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qaokResult)
            {
                Assert.True(qaokResult.Value != null);
                if (qaokResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task QueryReturnResponseAsync()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all
            var respQueryAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qokResult)
            {
                Assert.True(qokResult.Value != null);
                if (qokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> querycount)
                {
                    Assert.True(existingCount == querycount.Item.List.Count);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            respGetAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all again
            respQueryAll = await controller.QueryAsync(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qaokResult)
            {
                Assert.True(qaokResult.Value != null);
                if (qaokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Query_ByPropertiesAsync()
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseAsync(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQueryAll = await controller.QueryAsync(query);
                if (respQueryAll is OkObjectResult qaokResult)
                {
                    Assert.True(qaokResult.Value != null);
                    if (qaokResult.Value is ServiceQueryResponse<TDto> querycount)
                    {
                        Assert.True(querycount.List.Count >= 1);
                        var foundObject = TestManager.FindObject(querycount.List, dto);
                        Assert.True(foundObject != null);
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
        }

        [Fact]
        public virtual async Task Query_ByPropertiesReturnResponseAsync()
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBaseReturnResponseAsync(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQueryAll = await controller.QueryAsync(query);
                if (respQueryAll is OkObjectResult qaokResult)
                {
                    Assert.True(qaokResult.Value != null);
                    if (qaokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> querycount)
                    {
                        Assert.True(querycount.Item.List.Count >= 1);
                        var foundObject = TestManager.FindObject(querycount.Item.List, dto);
                        Assert.True(foundObject != null);
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
        }

        #endregion Async

        #region Sync

        [Fact]
        public virtual void Create_NullData()
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Create
            var respCreate = controller.Create(null);

            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Create_NullDataReturnResponse()
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            //Call Create
            var respCreate = controller.Create(null);

            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                {
                    Assert.True(resp.Error);
                    Assert.NotNull(resp.Messages);
                    Assert.True(resp.Messages.Count > 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        public virtual TDto CreateBase(TDto model)
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();

            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultGetAll)
            {
                Assert.True(okResultGetAll.Value != null);
                if (okResultGetAll.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
            }
            else
                Assert.Fail(JsonConvert.SerializeObject(respGetAll));

            //Call Create
            var respCreate = controller.Create(model);
            if (respCreate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Post);

                    //Call GetItem
                    var respGetItem = controller.Get(obj.StorageKey);
                    if (respGetItem is OkObjectResult okResultGetItem && model != null)
                    {
                        Assert.True(okResultGetItem.Value != null);
                        if (okResultGetItem.Value is TDto gobj)
                        {
                            //Validate
                            TestManager.ValidateObjects(model, gobj, HttpMethod.Post);

                            //Call GetAll
                            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
                            if (respGetAll is OkObjectResult okResultGetAll2)
                            {
                                Assert.True(okResultGetAll2.Value != null);
                                if (okResultGetAll2.Value is ServiceQueryResponse<TDto> sqr)
                                {
                                    Assert.True(sqr.List.Count == 1 + existingCount);
                                    var foundObject = TestManager.FindObject(sqr.List, gobj);
                                    Assert.True(foundObject != null);

                                    //Validate
                                    TestManager.ValidateObjects(gobj, foundObject, HttpMethod.Get);
                                    return gobj;
                                }
                                else
                                    Assert.Fail("");
                            }
                            else
                                Assert.Fail("");
                        }
                        else
                            Assert.Fail("");
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            throw new Exception();
        }

        public virtual TDto CreateBaseReturnResponse(TDto model)
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();

            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultGetAll)
            {
                Assert.True(okResultGetAll.Value != null);
                if (okResultGetAll.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
            }
            else
                Assert.Fail("");

            //Call Create
            var respCreate = controller.Create(model);
            if (respCreate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Post);

                    //Call GetItem
                    var respGetItem = controller.Get(resp.Item.StorageKey);
                    if (respGetItem is OkObjectResult okResultGetItem && model != null)
                    {
                        Assert.True(okResultGetItem.Value != null);
                        if (okResultGetItem.Value is ResponseItem<TDto> gobj)
                        {
                            //Validate
                            TestManager.ValidateObjects(model, gobj.Item, HttpMethod.Post);

                            //Call GetAll
                            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
                            if (respGetAll is OkObjectResult okResultGetAll2)
                            {
                                Assert.True(okResultGetAll2.Value != null);
                                if (okResultGetAll2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                                {
                                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                                    var foundObject = TestManager.FindObject(sqr.Item.List, gobj.Item);
                                    Assert.True(foundObject != null);

                                    //Validate
                                    TestManager.ValidateObjects(gobj.Item, foundObject, HttpMethod.Get);
                                    return gobj.Item;
                                }
                                else
                                    Assert.Fail("");
                            }
                            else
                                Assert.Fail("");
                        }
                        else
                            Assert.Fail("");
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            throw new Exception();
        }

        [Fact]
        public virtual void Create_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            CreateBase(model);
        }

        [Fact]
        public virtual void Create_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            CreateBaseReturnResponse(model);
        }

        [Fact]
        public virtual void Create_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            CreateBase(model);
        }

        [Fact]
        public virtual void Create_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();

            CreateBaseReturnResponse(model);
        }

        [Fact]
        public virtual void Create_Two()
        {
            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            Create_MinData();

            Create_MaxData();

            //Call GetAll again after create
            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 2 + existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Create_TwoReturnResponse()
        {
            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            Create_MinDataReturnResponse();

            Create_MaxDataReturnResponse();

            //Call GetAll again after create
            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 2 + existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAll_MinData()
        {
            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAll_MinDataReturnResponse()
        {
            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAll_MaxData()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAll_MaxDataReturnResponse()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetItem_NullData()
        {
            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = controller.Get(null);
            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetItem_NullDataReturnResponse()
        {
            //Call GetItem
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = controller.Get(null);
            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetItem_NotFound()
        {
            var model = TestManager.GetObjectNotFound();

            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = controller.Get(model.StorageKey);

            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");

            //Call GetItem with null
            respGetItem = controller.Get(null);

            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetItem_NotFoundReturnResponse()
        {
            var model = TestManager.GetObjectNotFound();

            //Call GetItem
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetItem = controller.Get(model.StorageKey);

            if (respGetItem is OkObjectResult okResult)
            {
                if (okResult.Value is Response resp)
                    Assert.True(resp.Success);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetItem with null
            respGetItem = controller.Get(null);

            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAllPaging_Multi()
        {
            Create_Two();

            int existingCount = 0;
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultAll)
            {
                Assert.True(okResultAll.Value != null);
                if (okResultAll.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    Assert.True(existingCount >= 2); //Possible pre-loaded data
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get one
            var paging = new Paging() { PageNumber = 1, PageSize = 1 };
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    Assert.True(respPaging.List.Count == 2);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult3)
            {
                Assert.True(okResult3.Value != null);
                if (okResult3.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2)
            {
                Assert.True(okResultpage2.Value != null);
                if (okResultpage2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2ofmax)
            {
                Assert.True(okResultpage2ofmax.Value != null);
                if (okResultpage2ofmax.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));

                    Assert.True(respPaging.List.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void GetAllPaging_MultiReturnResponse()
        {
            Create_TwoReturnResponse();

            int existingCount = 0;
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultAll)
            {
                Assert.True(okResultAll.Value != null);
                if (okResultAll.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    Assert.True(existingCount >= 2); //Possible pre-loaded data
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get one
            var paging = new Paging() { PageNumber = 1, PageSize = 1 };
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 2);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult3)
            {
                Assert.True(okResult3.Value != null);
                if (okResult3.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2)
            {
                Assert.True(okResultpage2.Value != null);
                if (okResultpage2.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, true).Build();
            respGetAll = controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2ofmax)
            {
                Assert.True(okResultpage2ofmax.Value != null);
                if (okResultpage2ofmax.Value is ResponseItem<ServiceQueryResponse<TDto>> respPaging)
                {
                    Assert.True(respPaging.Item.List != null);
                    if (respPaging.Item.List == null)
                        throw new ArgumentNullException(nameof(respPaging.Item.List));

                    Assert.True(respPaging.Item.List.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Update_NullData()
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = controller.Update(null);

            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Update_NullDataReturnResponse()
        {
            //Call Update
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = controller.Update(null);

            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is Response resp)
                    Assert.True(resp.Error);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual void UpdateNoChangeBase(TDto model)
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual void UpdateNoChangeBaseReturnResponse(TDto model)
        {
            //Call Update
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respUpdate = controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Update_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBase(model);

            UpdateNoChangeBase(dto);
        }

        [Fact]
        public virtual void Update_NoChange_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateNoChangeBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Update_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBase(model);

            UpdateNoChangeBase(dto);
        }

        [Fact]
        public virtual void Update_NoChange_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = CreateBaseReturnResponse(model);

            UpdateNoChangeBaseReturnResponse(dto);
        }

        protected virtual void UpdateBase(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual void UpdateBaseReturnResponse(TDto model)
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    //Validate
                    TestManager.ValidateObjects(model, resp.Item, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Update_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);
            UpdateBase(dto);
        }

        [Fact]
        public virtual void Update_MinDataReturnResponse()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Update_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);
            UpdateBase(dto);
        }

        [Fact]
        public virtual void Update_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);
            UpdateBaseReturnResponse(dto);
        }

        protected virtual void DeleteBase(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = controller.Delete(model.StorageKey);
            if (respDelete is OkObjectResult okResultDelete)
            {
                Assert.True(okResultDelete.Value != null);
                if (okResultDelete.Value is bool retVal)
                {
                    Assert.True(retVal == true);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify GetItem not found
            var respGetItem = controller.Get(model.StorageKey);
            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");
        }

        protected virtual void DeleteBaseReturnResponse(TDto model)
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = controller.Delete(model.StorageKey);
            if (respDelete is OkObjectResult okResultDelete)
            {
                Assert.True(okResultDelete.Value != null);
                if (okResultDelete.Value is Response retVal)
                {
                    Assert.True(retVal.Success);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify GetItem not found
            var respGetItem = controller.Get(model.StorageKey);
            if (respGetItem is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<TDto> resp)
                {
                    Assert.True(resp.Success);
                    Assert.True(resp.Item == null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Delete_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = CreateBase(model);

            DeleteBase(dto);
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
            var dto = CreateBase(model);

            DeleteBase(dto);
        }

        [Fact]
        public virtual void Delete_MaxDataReturnResponse()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            DeleteBaseReturnResponse(dto);
        }

        [Fact]
        public virtual void Query()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    existingCount = sqr.List.Count;
                    existingList = sqr.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all
            var respQueryAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qokResult)
            {
                Assert.True(qokResult.Value != null);
                if (qokResult.Value is ServiceQueryResponse<TDto> querycount)
                {
                    Assert.True(existingCount == querycount.List.Count);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all again
            respQueryAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qaokResult)
            {
                Assert.True(qaokResult.Value != null);
                if (qaokResult.Value is ServiceQueryResponse<TDto> sqr)
                {
                    Assert.True(sqr.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.List, dto);
                    Assert.True(foundObject != null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void QueryReturnResponse()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);
            var respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    existingCount = sqr.Item.List.Count;
                    existingList = sqr.Item.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all
            var respQueryAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qokResult)
            {
                Assert.True(qokResult.Value != null);
                if (qokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> querycount)
                {
                    Assert.True(existingCount == querycount.Item.List.Count);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            respGetAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all again
            respQueryAll = controller.Query(GetDefaultServiceQueryRequest());
            if (respQueryAll is OkObjectResult qaokResult)
            {
                Assert.True(qaokResult.Value != null);
                if (qaokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> sqr)
                {
                    Assert.True(sqr.Item.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(sqr.Item.List, dto);
                    Assert.True(foundObject != null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual void Query_ByProperties()
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBase(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQueryAll = controller.Query(query);
                if (respQueryAll is OkObjectResult qaokResult)
                {
                    Assert.True(qaokResult.Value != null);
                    if (qaokResult.Value is ServiceQueryResponse<TDto> querycount)
                    {
                        Assert.True(querycount.List.Count >= 1);
                        var foundObject = TestManager.FindObject(querycount.List, dto);
                        Assert.True(foundObject != null);
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
        }

        [Fact]
        public virtual void Query_ByPropertiesReturnResponse()
        {
            var controller = TestManager.GetControllerReturnResponse(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = CreateBaseReturnResponse(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQueryAll = controller.Query(query);
                if (respQueryAll is OkObjectResult qaokResult)
                {
                    Assert.True(qaokResult.Value != null);
                    if (qaokResult.Value is ResponseItem<ServiceQueryResponse<TDto>> querycount)
                    {
                        Assert.True(querycount.Item.List.Count >= 1);
                        var foundObject = TestManager.FindObject(querycount.Item.List, dto);
                        Assert.True(foundObject != null);
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }

            #endregion Sync
        }
    }
}