//using Microsoft.Extensions.DependencyInjection;

//namespace ServiceBricks.Xunit
//{
//    public class MappingTest
//    {
//        public virtual ISystemManager SystemManager { get; set; }

//        public MappingTest()
//        {
//            SystemManager = new SystemManager();
//            SystemManager?.StartSystem(typeof(TestStartup));
//        }

//        [Fact]
//        public virtual Task ValidateAutomapperConfiguration()
//        {
//            var mapper = SystemManager.ServiceProvider.GetRequiredService<AutoMapper.IMapper>();
//            mapper.ConfigurationProvider.AssertConfigurationIsValid();
//            return Task.CompletedTask;
//        }
//    }
//}