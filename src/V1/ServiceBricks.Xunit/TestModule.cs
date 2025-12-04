using System.Reflection;

namespace ServiceBricks.Xunit
{
    public class TestModule : Module
    {
        public static TestModule Instance = new TestModule();

        public TestModule()
        {
        }
    }
}