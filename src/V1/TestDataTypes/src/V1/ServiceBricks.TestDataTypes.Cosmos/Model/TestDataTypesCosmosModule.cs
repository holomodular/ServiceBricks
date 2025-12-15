using System.Reflection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Cosmos
{
    /// <summary>
    /// The module definition for the TestDataTypesCosmos module.
    /// </summary>
    public partial class TestDataTypesCosmosModule : ServiceBricks.Module
    {
        public static TestDataTypesCosmosModule Instance = new TestDataTypesCosmosModule();

        /// <summary>
        /// Constructor for the TestDataTypesCosmos module.
        /// </summary>
        public TestDataTypesCosmosModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesEntityFrameworkCoreModule.Instance
            };
        }
    }
}
