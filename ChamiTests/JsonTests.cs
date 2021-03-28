using ChamiUI.BusinessLayer;
using System.IO;
using Xunit;

namespace ChamiTests
{
    public class JsonTests
    {
        [Fact]
        public void TestReading()
        {
            var inputFile = "D:/code/Chami/ChamiTests/InputFiles/chami-sample.json";
            var stream = File.Open(inputFile, FileMode.Open);
            var environmentJsonReader = new EnvironmentJsonReader(stream);
            var readDocument = environmentJsonReader.Process();
            Assert.NotNull(readDocument.Name);
            Assert.NotEmpty(readDocument.Name);
        }
    }
}