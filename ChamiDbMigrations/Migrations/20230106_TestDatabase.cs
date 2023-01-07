using System.Collections.Generic;
using Chami.Db.Annotations;
using Chami.Db.Entities;
using Chami.Db.Migrations.Base;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202301062256)]
    public class TestDatabase : Migration, ITestMigration
    {
        public TestDatabase()
        {
            _environment = new 
            {
                Name = "Test environment",
                EnvironmentType = (int)EnvironmentType.NormalEnvironment,
                EnvironmentId = 1
            };

            _environmentVariables = new List<object>();
            for (int i = 1; i <= 3; i++)
            {
                var environmentVariable = new
                {
                    Name = "Test variable " + i, Value = "Value " + i + 1, EnvironmentId = 1, EnvironmentVariableId = i
                };
                _environmentVariables.Add(environmentVariable);
            }
        }
        private readonly List<object> _environmentVariables;
        private readonly object _environment;
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName<Environment>()).Row(_environment);

            foreach (var row in _environmentVariables)
            {
                Insert.IntoTable(AnnotationUtils.GetTableName<EnvironmentVariable>()).Row(row);
            }
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName<EnvironmentVariable>());
            Delete.FromTable(AnnotationUtils.GetTableName<Environment>());
        }
    }
}