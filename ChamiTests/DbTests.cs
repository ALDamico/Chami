using System.Linq;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using Xunit;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiTests
{
    public class DbTests
    {
        private static string connectionString = "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
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

            var environmentVariable1 = new EnvironmentVariable() { Name = "USER", Value = "TestUser" };
            var environmentVariable2 = new EnvironmentVariable() { Name = "PASSWORD", Value = "SECRET" };
            environment.EnvironmentVariables.Add(environmentVariable1);
            environment.EnvironmentVariables.Add(environmentVariable2);

            repository.InsertEnvironment(environment);
        }

        [Fact]
        public void GetExistingEnvironment()
        {
            var repository = new EnvironmentRepository(connectionString);
            var environment = repository.GetEnvironmentById(1);
            Assert.NotNull(environment);
            Assert.NotEmpty(environment.EnvironmentVariables);

            var environmentVariableId = environment.EnvironmentVariables.First().EnvironmentVariableId;
            Assert.NotEqual(0, environmentVariableId);
        }
    }
}