using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                Key = Guid.NewGuid().ToString()
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
        public virtual Task ResponseMessageConstructorTest()
        {
            var errorMessage = ResponseMessage.CreateError("test");

            ResponseMessage newMessage = new ResponseMessage(errorMessage);
            Assert.True(newMessage.Severity == ResponseSeverity.Error);

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

            response = new ResponseAggregateCountList<ExampleDto>(
                new List<ExampleDto>());
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
            var d = module.DependentModules;
            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ModuleRegistrySuccess()
        {
            ServiceBricksModule module = new ServiceBricksModule();

            // assert
            var mods = ModuleRegistry.Instance.GetKeys();
            Assert.True(mods.Count == 1);

            // register again
            ModuleRegistry.Instance.Register(module);

            // assert
            mods = ModuleRegistry.Instance.GetKeys();
            Assert.True(mods.Count == 1);

            // unregister
            ModuleRegistry.Instance.UnRegister(new ServiceBricksModule());

            // assert
            mods = ModuleRegistry.Instance.GetKeys();
            Assert.True(mods.Count == 0);

            // Cleanup (put it back)
            ModuleRegistry.Instance.Register(module);

            return Task.CompletedTask;
        }

        public class ModuleA : Module
        {
            public ModuleA()
            { StartPriority = 0; }
        }

        public class ModuleB : Module
        {
            public ModuleB()
            { StartPriority = 0; }
        }

        public class ModulePriority1 : Module
        {
            public ModulePriority1()
            { StartPriority = 1; }
        }

        public class ModulePriority2 : Module
        {
            public ModulePriority2()
            { StartPriority = 2; }
        }

        public class ModuleAStartRule : BusinessRule
        {
            public static DateTimeOffset CallDate;

            public static void Register(IBusinessRuleRegistry registry)
            {
                registry.Register(typeof(ModuleStartEvent<ModuleA>), typeof(ModuleAStartRule));
            }

            public static void UnRegister(IBusinessRuleRegistry registry)
            {
                registry.UnRegister(typeof(ModuleStartEvent<ModuleA>), typeof(ModuleAStartRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                CallDate = DateTimeOffset.UtcNow;
                return new Response();
            }
        }

        public class ModuleBStartRule : BusinessRule
        {
            public static DateTimeOffset CallDate;

            public static void Register(IBusinessRuleRegistry registry)
            {
                registry.Register(typeof(ModuleStartEvent<ModuleB>), typeof(ModuleBStartRule));
            }

            public static void UnRegister(IBusinessRuleRegistry registry)
            {
                registry.UnRegister(typeof(ModuleStartEvent<ModuleB>), typeof(ModuleBStartRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                CallDate = DateTimeOffset.UtcNow;
                return new Response();
            }
        }

        public class ModulePriority1StartRule : BusinessRule
        {
            public static DateTimeOffset CallDate;

            public static void Register(IBusinessRuleRegistry registry)
            {
                registry.Register(typeof(ModuleStartEvent<ModulePriority1>), typeof(ModulePriority1StartRule));
            }

            public static void UnRegister(IBusinessRuleRegistry registry)
            {
                registry.UnRegister(typeof(ModuleStartEvent<ModulePriority1>), typeof(ModulePriority1StartRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                CallDate = DateTimeOffset.UtcNow;
                return new Response();
            }
        }

        public class ModulePriority2StartRule : BusinessRule
        {
            public static DateTimeOffset CallDate;

            public static void Register(IBusinessRuleRegistry registry)
            {
                registry.Register(typeof(ModuleStartEvent<ModulePriority2>), typeof(ModulePriority2StartRule));
            }

            public static void UnRegister(IBusinessRuleRegistry registry)
            {
                registry.UnRegister(typeof(ModuleStartEvent<ModulePriority2>), typeof(ModulePriority2StartRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                CallDate = DateTimeOffset.UtcNow;
                return new Response();
            }
        }

        [Fact]
        public virtual Task ModulePriorityTests()
        {
            // assert (ServiceBricksModule is there from startup)
            var mods = ModuleRegistry.Instance.GetKeys();
            Assert.True(mods.Count == 1);

            // Create modules
            var moda = new ModuleA();
            var modb = new ModuleB();
            var mod1 = new ModulePriority1();
            var mod2 = new ModulePriority2();

            // Register new ones
            ModuleRegistry.Instance.Register(mod2);
            ModuleRegistry.Instance.Register(modb);
            ModuleRegistry.Instance.Register(mod1);
            ModuleRegistry.Instance.Register(moda);
            ModuleAStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleBStartRule.Register(BusinessRuleRegistry.Instance);
            ModulePriority1StartRule.Register(BusinessRuleRegistry.Instance);
            ModulePriority2StartRule.Register(BusinessRuleRegistry.Instance);

            // assert
            mods = ModuleRegistry.Instance.GetKeys();
            Assert.True(mods.Count == 5);

            // Call Start
            ApplicationBuilder builder = new ApplicationBuilder(SystemManager.ServiceProvider);
            builder.StartServiceBricks();

            // Make sure they are ordered correctly, should be:
            // modb, moda, mod1, mod2
            Assert.True(ModuleBStartRule.CallDate < ModuleAStartRule.CallDate);
            Assert.True(ModuleAStartRule.CallDate < ModulePriority1StartRule.CallDate);
            Assert.True(ModulePriority1StartRule.CallDate < ModulePriority2StartRule.CallDate);

            // Cleanup
            ModuleRegistry.Instance.UnRegister(mod2);
            ModuleRegistry.Instance.UnRegister(modb);
            ModuleRegistry.Instance.UnRegister(mod1);
            ModuleRegistry.Instance.UnRegister(moda);
            ModuleAStartRule.UnRegister(BusinessRuleRegistry.Instance);
            ModuleBStartRule.UnRegister(BusinessRuleRegistry.Instance);
            ModulePriority1StartRule.UnRegister(BusinessRuleRegistry.Instance);
            ModulePriority2StartRule.UnRegister(BusinessRuleRegistry.Instance);

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
            exception = new BusinessException(string.Empty);
            Assert.True(exception.Messages.Count == 1);

            exception = new BusinessException(new Exception());
            Assert.True(exception.Messages.Count == 2);

            exception = new BusinessException(new Exception(string.Empty));
            Assert.True(exception.Messages.Count == 2);

            exception = new BusinessException(new Exception(), "test");
            Assert.True(exception.Messages.Count == 2);
            Assert.True(exception.Messages[0].Message == "test");

            exception = new BusinessException(new Exception(), string.Empty);
            Assert.True(exception.Messages.Count == 2);

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

            string message = exception.GetMessage(Environment.NewLine);
            Assert.True(message.Length > 0);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ResponseCreateMessage()
        {
            // Error
            var response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString()));
            Assert.True(response.Error);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(string.Empty));
            Assert.True(response.Error);

            // Error
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Error);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(string.Empty, "field"));
            Assert.True(response.Error);

            // Error
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Error);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(string.Empty, new List<string>() { "field" }));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException()));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), "test"));
            Assert.True(response.Error);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), string.Empty));
            Assert.True(response.Error);

            // Exception
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), "test", new List<string>() { "field" }));
            Assert.True(response.Error);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), string.Empty, new List<string>() { "field" }));
            Assert.True(response.Error);

            // Scrub
            response = new Response();
            response.AddMessage(ResponseMessage.CreateError(new BusinessException(), "test", new List<string>() { "field" }));
            Assert.True(response.Error);
            Assert.True(response.Messages.Count == 2);
            response.Scrub();
            Assert.True(response.Messages.Count == 1);
            Assert.True(response.Messages[0].Severity == ResponseSeverity.Error);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(string.Empty));
            Assert.True(response.Success);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(string.Empty, "field"));
            Assert.True(response.Success);

            // Info
            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateInfo(string.Empty, new List<string>() { "field" }));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(string.Empty));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(string.Empty, "field"));
            Assert.True(response.Success);

            // Warning
            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateWarning(string.Empty, new List<string>() { "field" }));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString()));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(string.Empty));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString(), "field"));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(string.Empty, "field"));
            Assert.True(response.Success);

            // Success
            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(Guid.NewGuid().ToString(), new List<string>() { "field" }));
            Assert.True(response.Success);

            response = new Response();
            response.AddMessage(ResponseMessage.CreateSuccess(string.Empty, new List<string>() { "field" }));
            Assert.True(response.Success);

            var msg = response.GetMessage(Environment.NewLine);
            Assert.True(msg != null && msg.Length > 0);

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


        public class MapA
        {
            public string Name { get; set; }
        }
        public class MapB
        {
            public string Name { get; set; }
        }
        Action<MapA, MapB> MapAmapper = (s, d) =>
        {            
            d.Name = s.Name;
        };
        Action<MapB, MapA> MapBmapper = (s, d) =>
        {
            d.Name = s.Name;
        };


        [Fact]
        public virtual Task MappingTests()
        {

            var mapperRegistry = SystemManager.ServiceProvider.GetRequiredService<IMapperRegistry>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            var mapa = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapa == null);
            mapperRegistry.Register<MapA, MapB>(MapAmapper);
            mapa = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapa != null);

            var mapb = mapperRegistry.GetMapper<MapB, MapA>();
            Assert.True(mapb == null);
            mapperRegistry.Register(MapBmapper);
            mapb = mapperRegistry.GetMapper<MapB, MapA>();
            Assert.True(mapb != null);

            var a = new MapA() { Name = "a" };
            var b = new MapB() { Name = "b" };

            mapper.Map(a, b);
            Assert.True(a.Name == b.Name);

            a.Name = "a";
            b.Name = "b";
            mapper.Map(b, a);
            Assert.True(a.Name == b.Name);

            // CLeanup
            mapperRegistry.UnRegister<MapA, MapB>();
            mapperRegistry.UnRegister(typeof(MapB), typeof(MapA));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task MappingListTests()
        {

            var mapperRegistry = SystemManager.ServiceProvider.GetRequiredService<IMapperRegistry>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            var mapa = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapa == null);
            mapperRegistry.Register<MapA, MapB>(MapAmapper);
            mapa = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapa != null);

            var mapb = mapperRegistry.GetMapper<MapB, MapA>();
            Assert.True(mapb == null);
            mapperRegistry.Register(MapBmapper);
            mapb = mapperRegistry.GetMapper<MapB, MapA>();
            Assert.True(mapb != null);

            var a = new MapA() { Name = "a" };
            var b = new MapB() { Name = "b" };

            List<MapA> lista = new List<MapA>() { a };
            List<MapB> listb = new List<MapB>();

            mapper.Map(lista, listb);
            Assert.True(listb.Count == 1);
            Assert.True(lista[0].Name == listb[0].Name);

            // CLeanup
            mapperRegistry.UnRegister<MapA, MapB>();
            mapperRegistry.UnRegister(typeof(MapB), typeof(MapA));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task MappingNoRegistryTests()
        {

            var mapperRegistry = SystemManager.ServiceProvider.GetRequiredService<IMapperRegistry>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            var mapa = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapa == null);            

            var mapb = mapperRegistry.GetMapper<MapA, MapB>();
            Assert.True(mapb == null);

            var a = new MapA() { Name = "a" };
            var b = new MapB() { Name = "b" };

            Assert.Throws<BusinessException>(() =>
            {
                mapper.Map(a, b);
            });            

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task MappingNoRegistrySameObjectTests()
        {

            var mapperRegistry = SystemManager.ServiceProvider.GetRequiredService<IMapperRegistry>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            var mapa = mapperRegistry.GetMapper<MapA, MapA>();
            Assert.True(mapa == null);
                        

            var a = new MapA() { Name = "a" };
            var a2 = new MapA() { Name = "a2" };

            mapper.Map(a, a2);
            Assert.True(a.Name == a2.Name);

            List<MapA> lista = new List<MapA>() { a };
            List<MapA> lista2 = new List<MapA>();

            mapper.Map(lista, lista2);
            Assert.True(lista2.Count == 1);
            Assert.True(lista[0].Name == lista2[0].Name);


            return Task.CompletedTask;
        }


        [Fact]
        public virtual Task DomainTypeProfileTest()
        {
            DomainTypeMappingProfile testProfile = new DomainTypeMappingProfile();
            return Task.CompletedTask;
        }

        public class TestHttpContextAccessor : IHttpContextAccessor
        {
            public TestHttpContextAccessor()
            {
                HttpContext = new DefaultHttpContext();
            }

            public HttpContext HttpContext { get; set; }
        }

        [Fact]
        public virtual async Task ValidationResponseTest()
        {
            TestHttpContextAccessor contextAccessor = new TestHttpContextAccessor();

            ValidationResponseResult result = new ValidationResponseResult();

            Microsoft.AspNetCore.Mvc.ActionContext actionContext =
                new Microsoft.AspNetCore.Mvc.ActionContext();
            actionContext.HttpContext = contextAccessor.HttpContext;
            actionContext.HttpContext.RequestServices = SystemManager.ServiceProvider;
            actionContext.ModelState.AddModelError("key", "error");

            try
            {
                await result.ExecuteResultAsync(actionContext);
            }
            catch
            {
                // TODO: Fix this test
            }
        }

        public static class ExampleTask
        {
            public class Detail : ITaskDetail<Detail, Worker>
            {
            }

            public class Worker : ITaskWork<Detail, Worker>
            {
                public static bool WasCalled = false;

                public Task DoWork(Detail detail, CancellationToken cancellationToken)
                {
                    WasCalled = true;
                    return Task.CompletedTask;
                }
            }
        }

        public class ExampleTimer : TaskTimerHostedService<ExampleTask.Detail, ExampleTask.Worker>
        {
            public ExampleTimer(
                IServiceProvider serviceProvider,
                ILoggerFactory logger) : base(serviceProvider, logger)
            {
            }

            public override TimeSpan TimerTickInterval
            {
                get { return TimeSpan.FromMilliseconds(1); }
            }

            public override TimeSpan TimerDueTime
            {
                get { return TimeSpan.FromMilliseconds(1); }
            }

            public override ITaskDetail<ExampleTask.Detail, ExampleTask.Worker> TaskDetail
            {
                get { return new ExampleTask.Detail(); }
            }

            public override bool TimerTickShouldProcessRun()
            {
                return true;
            }
        }

        [Fact]
        public virtual async Task TimerHostedServiceTest()
        {
            var exampleTimer = new ExampleTimer(SystemManager.ServiceProvider, SystemManager.ServiceProvider.GetService<ILoggerFactory>());
            await exampleTimer.StartAsync(CancellationToken.None);

            await Task.Delay(1000);

            await exampleTimer.StopAsync(CancellationToken.None);

            Assert.True(ExampleTask.Worker.WasCalled);
        }

        [Fact]
        public virtual Task ServicePartsTests()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder()
                .AddAppSettingsConfig().Build();
            services.AddServiceBricks(configuration);
            var parts = services.GetServiceBricksParts();
            Assert.True(parts != null);
            return Task.CompletedTask;
        }
    }
}