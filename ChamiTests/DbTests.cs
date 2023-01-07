using System;
using System.Linq;
using Chami.Db.Entities;
using Chami.Db.Migrations.Base;
using Chami.Db.Repositories;
using ChamiDbMigrations.Migrations;
using ChamiTests.Fixtures;
using ChamiUI.Configuration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiTests
{
    public class DbTests : IClassFixture<DatabaseMigratorFixture>
    {
        public DbTests(DatabaseMigratorFixture databaseMigratorFixture)
        {
            _databaseMigratorFixture = databaseMigratorFixture;
        }

        private readonly DatabaseMigratorFixture _databaseMigratorFixture;
        private const string connectionString = "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
        [Fact]
        public void GetNotExistingEnvironment()
        {
            var repository = _databaseMigratorFixture.EnvironmentRepository;
            var shouldBeNull = repository.GetEnvironmentById(-1);
            Assert.Null(shouldBeNull);
        }

        [Fact]
        public void InsertTest()
        {
            var repository = _databaseMigratorFixture.EnvironmentRepository;
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
            var environment = repository.GetEnvironmentById(1);
            Assert.NotNull(environment);
            Assert.NotEmpty(environment.EnvironmentVariables);

            var environmentVariableId = environment.EnvironmentVariables.First().EnvironmentVariableId;
            Assert.NotEqual(0, environmentVariableId);
        }
    }
}