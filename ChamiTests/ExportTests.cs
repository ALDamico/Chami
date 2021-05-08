using System;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.DataLayer.Entities;
using Xunit;
using Environment = ChamiUI.DataLayer.Entities.Environment;

namespace ChamiTests
{
    public class ExportTests
    {
        [Fact]
        public void TestExcelExport()
        {
            using (var exporter = new EnvironmentExcelExporter())
            {
                var environment1 = new Environment() {Name = "Test", AddedOn = DateTime.Now, IsBackup = false};
                environment1.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                environment1.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                environment1.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                var environment2 = new Environment(){Name = "Test2", AddedOn = DateTime.Now, IsBackup = false};
                environment2.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                environment2.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                environment2.EnvironmentVariables.Add(new EnvironmentVariable(){Name = "USER", Value = "MyValue", AddedOn = DateTime.Now, Environment = environment1});
                exporter.AddEnvironment(environment1);
                exporter.AddEnvironment(environment2);
                exporter.Export("d:\\test.xlsx");
            }
        }
    }
}