using System.Reflection;

namespace ServiceBricks.TestDataTypes.MongoDb
{
    /// <summary>
    /// The module definition for the TestDataTypesMongoDb module.
    /// </summary>
    public partial class TestDataTypesMongoDbModule : ServiceBricks.Module
    {
        public static TestDataTypesMongoDbModule Instance = new TestDataTypesMongoDbModule();

        /// <summary>
        /// Constructor for the TestDataTypesMongoDb module.
        /// </summary>
        public TestDataTypesMongoDbModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesModule.Instance
            };
        }
    }
}
