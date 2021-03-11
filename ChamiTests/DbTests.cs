using System;
using ChamiUI.DataLayer.Entities;
using ChamiUI.DataLayer.Repositories;
using Xunit;
using Environment = ChamiUI.DataLayer.Entities.Environment;

namespace ChamiTests
{
    public class DbTests
    {
        private static string connectionString = "Data Source=C:/Users/aldam/RiderProjects/Chami/ChamiUI/DataLayer/Db/chami.db;Version=3;";
        [Fact]
        public void GetNotExistingEnvironment()
        {
            var repository = new EnvironmentRepository(connectionString);
            var shouldBeNull = repository.GetEnvironmentById(-1);
            Assert.Null(shouldBeNull);
        }

        [Fact]
        public void InsertTest()
        {
            var repository = new EnvironmentRepository(connectionString);

            var environment = new Environment()
            {
                Name = "Example"
            };

            var environmentVariable1 = new EnvironmentVariable() {Name = "USER", Value = "TestUser"};
            var environmentVariable2 = new EnvironmentVariable() {Name = "PASSWORD", Value = "SECRET"};
            environment.EnvironmentVariables.Add(environmentVariable1);
            environment.EnvironmentVariables.Add(environmentVariable2);

            repository.InsertEnvironment(environment);
        }
    }
}