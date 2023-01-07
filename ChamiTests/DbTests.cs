using System;
using System.Linq;
using Chami.Db.Entities;
using Chami.Db.Migrations.Base;
using Chami.Db.Repositories;
using ChamiDbMigrations.Migrations;
using ChamiUI.Configuration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiTests
{
    public class DbTests
    {
        public DbTests()
        {
            ServiceProvider serviceProvider = new ServiceCollection().ConfigureFluentMigrator(connectionString, typeof(ITestMigration)).BuildServiceProvider();

            _migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
            _migrationRunner.MigrateDown(0);
            _migrationRunner.MigrateUp();

        }
        private const string connectionString = "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
        private readonly IMigrationRunner _migrationRunner;
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
            var env = repository.GetEnvironmentByName("Example");
            if (env != null)
            {
                repository.DeleteEnvironmentById(env.EnvironmentId);
            }

            var environment = new Environment()
            {
                Name = "Example"
            };

            var environmentVariable1 = new EnvironmentVariable() { Name = "USER", Value = "TestUser" };
            var environmentVariable2 = new EnvironmentVariable() { Name = "PASSWORD", Value = "SECRET" };
            environment.EnvironmentVariables.Add(environmentVariable1);
            environment.EnvironmentVariables.Add(environmentVariable2);

            var inserted = repository.InsertEnvironment(environment);
            Assert.NotNull(inserted);
            Assert.True(inserted.EnvironmentId > 0);
        }

        [Fact]
        public void GetExistingEnvironment()
        {
            var repository = new EnvironmentRepository(connectionString);
            var environments = repository.GetEnvironments();
            var environment = repository.GetEnvironmentById(1);
            Assert.NotNull(environment);
            Assert.NotEmpty(environment.EnvironmentVariables);

            var environmentVariableId = environment.EnvironmentVariables.First().EnvironmentVariableId;
            Assert.NotEqual(0, environmentVariableId);
        }
    }
}