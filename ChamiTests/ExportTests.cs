using System;
using System.IO;
using System.Threading.Tasks;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Exporters;
using Microsoft.VisualBasic.FileIO;
using Xunit;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiTests
{
    public sealed class ExportTests : IDisposable
    {
        private readonly string _asyncFileLocation = SpecialDirectories.MyDocuments + "/test_async.xlsx";
        private readonly string _fileLocation = SpecialDirectories.MyDocuments + "/test.xlsx";
        [Fact]
        public void TestExcelExport()
        {
            using (var exporter = new EnvironmentExcelExporter())
            {
                ConfigureExporter(exporter);
                exporter.Export("test.xlsx");
                Assert.True(File.Exists(_fileLocation));
            }
        }

        [Fact]
        public async Task TestExcelExportAsync()
        {
            using (var exporter = new EnvironmentExcelExporter())
            {
                ConfigureExporter(exporter);
                await exporter.ExportAsync("test_async.xlsx");
                Assert.True(File.Exists(_asyncFileLocation));
            }
        }

        private static void ConfigureExporter(EnvironmentExcelExporter exporter)
        {
            var environment1 = new Environment() {Name = "Test", AddedOn = DateTime.Now, EnvironmentType = 0};
            environment1.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            environment1.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            environment1.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            var environment2 = new Environment() {Name = "Test2", AddedOn = DateTime.Now, EnvironmentType = 0};
            environment2.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            environment2.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            environment2.EnvironmentVariables.Add(new EnvironmentVariable()
                {Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
            exporter.AddEnvironment(environment1);
            exporter.AddEnvironment(environment2);
        }

        public void Dispose()
        {
            File.Delete(_asyncFileLocation);
            File.Delete(_fileLocation);
            GC.SuppressFinalize(this);
        }
    }
}
