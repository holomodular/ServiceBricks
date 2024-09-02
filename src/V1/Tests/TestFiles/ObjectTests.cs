using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class DtoTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public DtoTests()
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
    }
}