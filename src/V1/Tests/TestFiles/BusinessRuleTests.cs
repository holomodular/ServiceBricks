using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class BusinessRuleTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public BusinessRuleTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        private class SuccessBusinessRule : BusinessRule
        {
            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                return new Response();
            }
        }

        private class ErrorBusinessRule : BusinessRule
        {
            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                var response = new Response();
                response.AddMessage(ResponseMessage.CreateError("Error"));
                return response;
            }
        }

        private class ExcludeErrorBusinessRule : BusinessRuleExclusion
        {
            public ExcludeErrorBusinessRule()
            {
                this.ExcludedDomainRuleKeys.Add(typeof(ErrorBusinessRule).FullName);
            }
        }

        private class ErrorDontStopBusinessRule : BusinessRule
        {
            public ErrorDontStopBusinessRule()
            {
                Priority = -1;
                StopOnError = false;
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                var response = new Response();
                response.AddMessage(ResponseMessage.CreateError("Error"));
                return response;
            }
        }

        [Fact]
        public virtual Task RegisterOne()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            var list = registry.GetKeys();
            Assert.True(list.Count == 1);

            var vals = registry.GetRegistryList(typeof(ExampleDto));
            Assert.True(vals.Count == 1);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task RegisterTwo()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));

            var list = registry.GetKeys();
            Assert.True(list.Count == 1);

            var vals = registry.GetRegistryList(typeof(ExampleDto));
            Assert.True(vals.Count == 2);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task RegisterDuplicate()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            var list = registry.GetKeys();
            Assert.True(list.Count == 1);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task UnRegister()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            registry.UnRegister(typeof(ExampleDto));
            var list = registry.GetKeys();
            Assert.True(list.Count == 0);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task UnRegisterItem()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            registry.UnRegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            var list = registry.GetKeys();
            Assert.True(list.Count == 0);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task RegisterTwoUnRegisterOne()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));
            registry.UnRegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));
            var list = registry.GetKeys();
            Assert.True(list.Count == 1);

            var vals = registry.GetRegistryList(typeof(ExampleDto));
            Assert.True(vals.Count == 1, $"Count is {vals.Count}");

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ServiceSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = businessRuleService.ExecuteRules(context);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ServiceSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(SuccessBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = await businessRuleService.ExecuteRulesAsync(context);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));
        }

        [Fact]
        public virtual Task ServiceError()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = businessRuleService.ExecuteRules(context);
            Assert.True(response.Error);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ServiceErrorAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = await businessRuleService.ExecuteRulesAsync(context);
            Assert.True(response.Error);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));
        }

        [Fact]
        public virtual Task ServiceErrorExcludedBuiltIn()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ExcludeErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = businessRuleService.ExecuteRules(context);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ServiceErrorExcludedBuiltInAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ExcludeErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = await businessRuleService.ExecuteRulesAsync(context);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));
        }

        [Fact]
        public virtual Task ServiceErrorExcludedAdditional()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            List<IBusinessRule> additionalrules = new List<IBusinessRule>();
            additionalrules.Add(new ExcludeErrorBusinessRule());

            var response = businessRuleService.ExecuteRules(context, additionalrules);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ServiceErrorExcludedAdditionalAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            List<IBusinessRule> additionalrules = new List<IBusinessRule>();
            additionalrules.Add(new ExcludeErrorBusinessRule());

            var response = await businessRuleService.ExecuteRulesAsync(context, additionalrules);
            Assert.True(response.Success);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));
        }

        [Fact]
        public virtual Task ServiceErrorDontStop()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorDontStopBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = businessRuleService.ExecuteRules(context);
            Assert.True(response.Error);
            Assert.True(response.Messages.Count == 2);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ServiceErrorDontStopAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorBusinessRule));
            registry.RegisterItem(typeof(ExampleDto), typeof(ErrorDontStopBusinessRule));

            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ExampleDto();

            var response = await businessRuleService.ExecuteRulesAsync(context);
            Assert.True(response.Error);
            Assert.True(response.Messages.Count == 2);

            // Cleanup
            registry.UnRegister(typeof(ExampleDto));
        }

        [Fact]
        public virtual Task ApiConcurrencyByStringRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.RegisterRule(registry, nameof(ExampleDomain.Name));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);

            // Cleanup
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiConcurrencyByStringRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.RegisterRule(registry, nameof(ExampleDomain.Name));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);

            // Cleanup
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task ApiConcurrencyByStringRuleError()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.RegisterRule(registry, nameof(ExampleDomain.Name));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "DifferentName", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Error);

            // Cleanup
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiConcurrencyByStringRuleErrorAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.RegisterRule(registry, nameof(ExampleDomain.Name));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "DifferentName", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Error);

            // Cleanup
            ApiConcurrencyByStringRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task ApiConcurrencyByUpdateDateRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);

            // Cleanup
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiConcurrencyByUpdateDateRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);

            // Cleanup
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task ApiConcurrencyByUpdateDateRuleError()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            DateTimeOffset later = now.AddSeconds(1);
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = later });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Error);

            // Cleanup
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiConcurrencyByUpdateDateRuleErrorAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            DateTimeOffset later = now.AddSeconds(1);
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateBeforeEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = later });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Error);

            // Cleanup
            ApiConcurrencyByUpdateDateRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
        }

        public sealed class ExampleCreatedBroadcastRule : BusinessRule
        {
            public static bool WasExecuted { get; set; }

            public ExampleCreatedBroadcastRule()
            {
            }

            public static void RegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Subscribe(
                    typeof(CreatedBroadcast<ExampleDto>),
                    typeof(ExampleCreatedBroadcastRule));
            }

            public static void UnRegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Unsubscribe(
                    typeof(CreatedBroadcast<ExampleDto>),
                    typeof(ExampleCreatedBroadcastRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                var response = new Response();

                try
                {
                    var e = context.Object as CreatedBroadcast<ExampleDto>;
                    if (e == null || e.DomainObject == null)
                        return response;

                    WasExecuted = true;
                    return response;
                }
                catch
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }
            }
        }

        public sealed class ExampleUpdatedBroadcastRule : BusinessRule
        {
            public static bool WasExecuted { get; set; }

            public ExampleUpdatedBroadcastRule()
            {
            }

            public static void RegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Subscribe(
                    typeof(UpdatedBroadcast<ExampleDto>),
                    typeof(ExampleUpdatedBroadcastRule));
            }

            public static void UnRegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Unsubscribe(
                    typeof(UpdatedBroadcast<ExampleDto>),
                    typeof(ExampleUpdatedBroadcastRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                var response = new Response();

                try
                {
                    var e = context.Object as UpdatedBroadcast<ExampleDto>;
                    if (e == null || e.DomainObject == null)
                        return response;

                    WasExecuted = true;
                    return response;
                }
                catch
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }
            }
        }

        public sealed class ExampleDeletedBroadcastRule : BusinessRule
        {
            public static bool WasExecuted { get; set; }

            public ExampleDeletedBroadcastRule()
            {
            }

            public static void RegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Subscribe(
                    typeof(DeletedBroadcast<ExampleDto>),
                    typeof(ExampleDeletedBroadcastRule));
            }

            public static void UnRegisterServiceBus(IServiceBus serviceBus)
            {
                serviceBus.Unsubscribe(
                    typeof(DeletedBroadcast<ExampleDto>),
                    typeof(ExampleDeletedBroadcastRule));
            }

            public override IResponse ExecuteRule(IBusinessRuleContext context)
            {
                var response = new Response();

                try
                {
                    var e = context.Object as DeletedBroadcast<ExampleDto>;
                    if (e == null || e.DomainObject == null)
                        return response;

                    WasExecuted = true;
                    return response;
                }
                catch
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
                    return response;
                }
            }
        }

        [Fact]
        public virtual Task ApiCreatedBroadcastRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiCreatedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleCreatedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiCreateAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleCreatedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleCreatedBroadcastRule.WasExecuted);

            // Cleanup
            ApiCreatedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleCreatedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleCreatedBroadcastRule.WasExecuted = false;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiCreatedBroadcastRuleSuccessAsync()

        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiCreatedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleCreatedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiCreateAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleCreatedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleCreatedBroadcastRule.WasExecuted);

            // Cleanup
            ApiCreatedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleCreatedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleCreatedBroadcastRule.WasExecuted = false;
        }

        [Fact]
        public virtual Task ApiUpdatedBroadcastRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiUpdatedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleUpdatedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleUpdatedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleUpdatedBroadcastRule.WasExecuted);

            // Cleanup
            ApiUpdatedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleUpdatedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleUpdatedBroadcastRule.WasExecuted = false;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiUpdatedBroadcastRuleSuccessAsync()

        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiUpdatedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleUpdatedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiUpdateAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleUpdatedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleUpdatedBroadcastRule.WasExecuted);

            // Cleanup
            ApiUpdatedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleUpdatedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleUpdatedBroadcastRule.WasExecuted = false;
        }

        [Fact]
        public virtual Task ApiDeletedBroadcastRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiDeletedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleDeletedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiDeleteAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleDeletedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleDeletedBroadcastRule.WasExecuted);

            // Cleanup
            ApiDeletedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleDeletedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleDeletedBroadcastRule.WasExecuted = false;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiDeletedBroadcastRuleSuccessAsync()

        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var servicebus = SystemManager.ServiceProvider.GetRequiredService<IServiceBus>();
            // Register rule
            ApiDeletedBroadcastRule<ExampleDomain, ExampleDto>.RegisterRule(registry);
            ExampleDeletedBroadcastRule.RegisterServiceBus(servicebus);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            BusinessRuleContext context = new BusinessRuleContext();
            context.Object = new ApiDeleteAfterEvent<ExampleDomain, ExampleDto>(
                new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now },
                new ExampleDto() { StorageKey = "1", Name = "Name", UpdateDate = now });

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            while (!ExampleDeletedBroadcastRule.WasExecuted)
            {
                if (cts.Token.IsCancellationRequested)
                    break;
            }

            Assert.True(ExampleDeletedBroadcastRule.WasExecuted);

            // Cleanup
            ApiDeletedBroadcastRule<ExampleDomain, ExampleDto>.UnRegisterRule(registry);
            ExampleDeletedBroadcastRule.UnRegisterServiceBus(servicebus);
            ExampleDeletedBroadcastRule.WasExecuted = false;
        }

        [Fact]
        public virtual Task DomainCreateDateRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainCreateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.CreateDate > now); // Always sets a new value on create

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate == now); // Does not change the value
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainCreateDateRule<ExampleDomain>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainCreateDateRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainCreateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.CreateDate > now); // Always sets a new value on create

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate == now); // Does not change the value
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainCreateDateRule<ExampleDomain>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task DomainCreateUpdateDateRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainCreateUpdateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate > now); // Always sets a new value on create
            Assert.True(domain.UpdateDate > now); // Always sets a new value on create
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            domain.UpdateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate == now); // Does not change the value
            Assert.True(domain.UpdateDate > now); // Always sets a new value on update
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainCreateUpdateDateRule<ExampleDomain>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainCreateUpdateDateRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainCreateUpdateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate > now); // Always sets a new value on create
            Assert.True(domain.UpdateDate > now); // Always sets a new value on create
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            domain.UpdateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.CreateDate == now); // Does not change the value
            Assert.True(domain.UpdateDate > now); // Always sets a new value on update
            Assert.True(domain.CreateDate.Offset == TimeSpan.Zero);
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainCreateUpdateDateRule<ExampleDomain>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task DomainUpdateDateRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainUpdateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.UpdateDate > now); // Always sets a new value on create
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            domain.UpdateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.UpdateDate > now); // Always sets a new value on update
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainUpdateDateRule<ExampleDomain>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainUpdateDateRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainUpdateDateRule<ExampleDomain>.RegisterRule(registry);

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.UpdateDate > now); // Always sets a new value on create
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.CreateDate = now;
            domain.UpdateDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(domain.UpdateDate > now); // Always sets a new value on update
            Assert.True(domain.UpdateDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainUpdateDateRule<ExampleDomain>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task DomainDateTimeOffsetRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainDateTimeOffsetRule<ExampleDomain>.RegisterRule(registry, nameof(ExampleDomain.ExampleDate));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now, ExampleDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(now.Offset != TimeSpan.Zero);
            Assert.True(domain.ExampleDate == now);
            Assert.True(domain.ExampleDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.ExampleDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(now.Offset != TimeSpan.Zero);
            Assert.True(domain.ExampleDate == now);
            Assert.True(domain.ExampleDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainDateTimeOffsetRule<ExampleDomain>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainDateTimeOffsetRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainDateTimeOffsetRule<ExampleDomain>.RegisterRule(registry, nameof(ExampleDomain.ExampleDate));

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now, ExampleDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do Create
            context.Object = new DomainCreateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(now.Offset != TimeSpan.Zero);
            Assert.True(domain.ExampleDate == now);
            Assert.True(domain.ExampleDate.Offset == TimeSpan.Zero);

            // Do Update
            now = DateTimeOffset.Now;
            domain.ExampleDate = now;
            context.Object = new DomainUpdateBeforeEvent<ExampleDomain>(domain);

            // Execute rule
            response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(now.Offset != TimeSpan.Zero);
            Assert.True(domain.ExampleDate == now);
            Assert.True(domain.ExampleDate.Offset == TimeSpan.Zero);

            // Cleanup
            DomainDateTimeOffsetRule<ExampleDomain>.UnRegisterRule(registry);
        }

        [Fact]
        public virtual Task DomainQueryPropertyRenameRuleSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainQueryPropertyRenameRule<ExampleDomain>.RegisterRule(registry, "TestName", "NewName");

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now, ExampleDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do query
            ServiceQueryRequest sqr = new ServiceQueryRequestBuilder()
                .IsEqual("TestName", "hello").Build();
            context.Object = new DomainQueryBeforeEvent<ExampleDomain>(sqr);
            Assert.True(sqr.Filters[0].Properties[0] == "TestName");

            // Execute rule
            var response = businessRuleService.ExecuteRules(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(sqr.Filters[0].Properties[0] == "NewName");

            // Cleanup
            DomainQueryPropertyRenameRule<ExampleDomain>.UnRegisterRule(registry);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainQueryPropertyRenameRuleSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Register rule
            DomainQueryPropertyRenameRule<ExampleDomain>.RegisterRule(registry, "TestName", "NewName");

            // Create context
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(1));
            var domain = new ExampleDomain() { Key = 1, Name = "Name", CreateDate = now, UpdateDate = now, ExampleDate = now };
            BusinessRuleContext context = new BusinessRuleContext();

            // Do query
            ServiceQueryRequest sqr = new ServiceQueryRequestBuilder()
                .IsEqual("TestName", "hello").Build();
            context.Object = new DomainQueryBeforeEvent<ExampleDomain>(sqr);
            Assert.True(sqr.Filters[0].Properties[0] == "TestName");

            // Execute rule
            var response = await businessRuleService.ExecuteRulesAsync(context);

            // Assert
            Assert.True(response.Success);
            Assert.True(sqr.Filters[0].Properties[0] == "NewName");

            // Cleanup
            DomainQueryPropertyRenameRule<ExampleDomain>.UnRegisterRule(registry);
        }
    }
}