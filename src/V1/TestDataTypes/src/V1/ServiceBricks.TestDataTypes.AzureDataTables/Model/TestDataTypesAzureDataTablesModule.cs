using System.Reflection;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// The module definition for the TestDataTypesAzureDataTables module.
    /// </summary>
    public partial class TestDataTypesAzureDataTablesModule : ServiceBricks.Module
    {
        public static TestDataTypesAzureDataTablesModule Instance = new TestDataTypesAzureDataTablesModule();

        /// <summary>
        /// Constructor for the TestDataTypesAzureDataTables module.
        /// </summary>
        public TestDataTypesAzureDataTablesModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesModule.Instance
            };
        }
    }
}
