using ChamiUI.BusinessLayer;
using Xunit;

namespace ChamiTests
{
    public class RegistryRetrieverTests
    {
        [Fact]
        public void TestGetEnvironment()
        {
            var registryRetriever = new EnvironmentVariableRegistryRetriever();
            var variables = registryRetriever.GetEnvironmentVariables();
            Assert.NotNull(variables);
            Assert.NotEmpty(variables);
        }

        [Fact]
        public void TestGetEnvironmentVariable()
        {
            string varName = "path";
            var registryRetriever = new EnvironmentVariableRegistryRetriever();
            var value = registryRetriever.GetEnvironmentVariable(varName);
            Assert.NotNull(value);
        }
    }
}