using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chami.Db.Migrations.Base;
using ChamiDbMigrations.Migrations;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Configuration
{
    public static class MigrationConfigurationExtensions
    {
        public static ServiceCollection ConfigureFluentMigrator(this ServiceCollection serviceCollection, string connectionString, params Type[] baseMigrations)
        {
            serviceCollection.AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(connectionString).ScanIn(typeof(Initial).Assembly).For
                        .Migrations()
                );

            var tagNames = new List<string>();
            foreach (var baseMigration in baseMigrations)
            {
                var tagsAttribute = (TagsAttribute) baseMigration.GetCustomAttribute(typeof(TagsAttribute), true);
                if (tagsAttribute != null)
                {
                    foreach (var tagName in tagsAttribute.TagNames)
                    {
                        tagNames.Add(tagName);
                    }
                   
                }
            }

            serviceCollection.Configure<RunnerOptions>(opts => opts.Tags = tagNames.Distinct().ToArray());

            return serviceCollection;
        }
    }
}