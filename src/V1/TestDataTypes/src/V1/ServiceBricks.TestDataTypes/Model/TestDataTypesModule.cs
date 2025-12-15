namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// The module definition for the TestDataTypes module.
    /// </summary>
    public partial class TestDataTypesModule : ServiceBricks.Module
    {
        public static TestDataTypesModule Instance = new TestDataTypesModule();

        public TestDataTypesModule()
        {
            DataTransferObjects = new List<Type>()
            {
                
                typeof(TestDto),
            };
        }
    }
}
