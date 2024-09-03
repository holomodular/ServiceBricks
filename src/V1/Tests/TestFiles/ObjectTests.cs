using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ObjectTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ObjectTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        [Fact]
        public virtual Task ApplicationEmailSuccess()
        {
            ApplicationEmailDto dto = new ApplicationEmailDto
            {
                BccAddress = Guid.NewGuid().ToString(),
                CcAddress = Guid.NewGuid().ToString(),
                FromAddress = Guid.NewGuid().ToString(),
                Subject = Guid.NewGuid().ToString(),
                ToAddress = Guid.NewGuid().ToString(),
                Body = Guid.NewGuid().ToString(),
                BodyHtml = Guid.NewGuid().ToString(),
                FutureProcessDate = DateTimeOffset.Now,
                IsHtml = true,
                Priority = Guid.NewGuid().ToString(),
                StorageKey = Guid.NewGuid().ToString(),
            };

            var broadcast = new CreateApplicationEmailBroadcast(dto);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ApplicationLogSuccess()
        {
            ApplicationLogDto dto = new ApplicationLogDto
            {
                Application = Guid.NewGuid().ToString(),
                Category = Guid.NewGuid().ToString(),
                CreateDate = DateTimeOffset.Now,
                Exception = Guid.NewGuid().ToString(),
                Level = Guid.NewGuid().ToString(),
                Message = Guid.NewGuid().ToString(),
                Path = Guid.NewGuid().ToString(),
                Properties = Guid.NewGuid().ToString(),
                Server = Guid.NewGuid().ToString(),
                StorageKey = Guid.NewGuid().ToString(),
                UserStorageKey = Guid.NewGuid().ToString(),
            };

            var broadcast = new CreateApplicationLogBroadcast(dto);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ApplicationSmsSuccess()
        {
            ApplicationSmsDto dto = new ApplicationSmsDto
            {
                FutureProcessDate = DateTimeOffset.Now,
                Message = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                StorageKey = Guid.NewGuid().ToString(),
            };

            var broadcast = new CreateApplicationSmsBroadcast(dto);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainTypeDtoSuccess()
        {
            DomainTypeDto dto = new DomainTypeDto
            {
                Name = Guid.NewGuid().ToString(),
                StorageKey = Guid.NewGuid().ToString(),
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainTypeSuccess()
        {
            DomainType dto = new DomainType
            {
                Name = Guid.NewGuid().ToString(),
                Key = 1
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseListSuccess()
        {
            ResponseList<ExampleDto> response = new ResponseList<ExampleDto>();
            response.List = new List<ExampleDto>();

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseCountSuccess()
        {
            ResponseCount response = new ResponseCount();
            response.Count = 1;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseAggregateCountListSuccess()
        {
            ResponseAggregateCountList<ExampleDto> response = new ResponseAggregateCountList<ExampleDto>();
            response.Aggregate = 1;
            response.List = new List<ExampleDto>();
            response.Count = 1;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ServicebusRegistrationSuccess()
        {
            var test = new ServiceBusRuleRegistration<CreateApplicationLogBroadcast>(
                new CreateApplicationLogBroadcast(new ApplicationLogDto()));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ModuleSuccess()
        {
            ServiceBricksModule module = new ServiceBricksModule();
            var a = module.ViewAssemblies;
            var auto = module.AutomapperAssemblies;
            var d = module.DependentModules;
            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DataTransferObjectSuccess()
        {
            DataTransferObject dto = new DataTransferObject
            {
                StorageKey = Guid.NewGuid().ToString(),
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainBroadcastSuccess()
        {
            DomainBroadcast dto = new DomainBroadcast
            {
                DomainObject = Guid.NewGuid().ToString(),
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainEventSuccess()
        {
            DomainEvent dto = new DomainEvent
            {
                DomainObject = Guid.NewGuid().ToString(),
            };

            return Task.CompletedTask;
        }

        public class TestClass : DomainObject<TestClass>
        {
            public int Key { get; set; }
        }

        [Fact]
        public virtual Task DomainObjectSuccess()
        {
            TestClass dto = new TestClass
            {
                Key = 1
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainProcessSuccess()
        {
            DomainProcess dto = new DomainProcess
            {
                DomainObject = Guid.NewGuid().ToString(),
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task DomainProcessGenericSuccess()
        {
            DomainProcess<ExampleDto> dto = new DomainProcess<ExampleDto>
            {
                DomainObject = new ExampleDto()
            };

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ApplicationOptionsSuccess()
        {
            ApplicationOptions options = new ApplicationOptions()
            {
                Name = Guid.NewGuid().ToString(),
            };
            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task BusinessExceptionSuccess()
        {
            BusinessException exception = new BusinessException();

            exception = new BusinessException(Guid.NewGuid().ToString());
            Assert.True(exception.Messages.Count == 1);

            exception = new BusinessException(new Exception());
            Assert.True(exception.Messages.Count == 2);

            exception = new BusinessException(new Exception(), "test");
            Assert.True(exception.Messages.Count == 2);
            Assert.True(exception.Messages[0].Message == "test");

            var response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo("test"));
            exception = new BusinessException(response);
            Assert.True(exception.Messages.Count == 1);
            Assert.True(exception.Messages[0].Message == "test");

            exception = new BusinessException(ResponseMessage.CreateWarning("test"));
            Assert.True(exception.Messages.Count == 1);
            Assert.True(exception.Messages[0].Message == "test");

            exception = new BusinessException();
            response = new Response();
            exception.CopyTo(response);
            Assert.True(response.Error);

            exception = new BusinessException();
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo("test"));
            exception.CopyFrom(response);
            Assert.True(exception.Messages.Count == 2);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseCreateMessage()
        {
            // Error
            var response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString()));
            Assert.True(response.Error);

            // Error
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Error);

            // Error
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException()));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), "test"));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), "test", new List<string>() { "field" }));
            Assert.True(response.Error);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseCopy()
        {
            // Error
            var responseerror = new Response();
            responseerror.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString()));
            Assert.True(responseerror.Error);

            // Success
            var response = new Response();
            Assert.True(response.Success);

            // Copy Error
            response.CopyFrom(responseerror);
            Assert.True(response.Error);

            // Error
            responseerror = new Response();
            responseerror.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString()));
            Assert.True(responseerror.Error);

            // Success
            response = new Response();
            Assert.True(response.Success);

            // Copy Error
            responseerror.CopyTo(response);
            Assert.True(response.Error);

            return Task.CompletedTask;
        }
    }
}