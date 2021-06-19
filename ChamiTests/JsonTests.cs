using System.Collections.Generic;
using ChamiUI.BusinessLayer;
using System.IO;
using ChamiUI.PresentationLayer.ViewModels;
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

        [Fact]
        public void TestReadingMultiple()
        {
            var inputFile = "D:/code/Chami/ChamiTests/InputFiles/chami-sample-multiple.json";
            var stream = File.Open(inputFile, FileMode.Open);
            var environmentJsonReader = new EnvironmentJsonReader(stream);
            List<EnvironmentViewModel> readDocuments = environmentJsonReader.ProcessMultiple();
            foreach (var readDocument in readDocuments)
            {
                Assert.NotNull(readDocument.Name);
                Assert.NotEmpty(readDocument.Name);
            }
        }
    }
}