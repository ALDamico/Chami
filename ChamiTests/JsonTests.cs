using System.Collections.Generic;
using ChamiUI.BusinessLayer;
using System.IO;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ChamiTests
{
    public class JsonTests
    {
        [Fact]
        public void TestWriting()
        {
            var outputFile = "D:/code/Chami/ChamiTests/OutputFiles/chami-output-test.json";
            var environment = new EnvironmentViewModel();
            environment.Name = "Test environment";
            var environmentVariable = new EnvironmentVariableViewModel();
            environmentVariable.Name = "USER";
            environmentVariable.Value = "ChamiUser";
            var environmentVariable2 = new EnvironmentVariableViewModel();
            environmentVariable2.Name = "PASSWORD";
            environmentVariable2.Value = "Passw0rd!";
            environment.EnvironmentVariables.Add(environmentVariable);
            environment.EnvironmentVariables.Add(environmentVariable2);
            var json = JsonConvert.SerializeObject(environment, Formatting.Indented, new EnvironmentViewModelJsonConverter());
            File.WriteAllText(outputFile, json);
            FileInfo fileInfo = new FileInfo(outputFile);
            Assert.Equal(124, fileInfo.Length);
        }
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