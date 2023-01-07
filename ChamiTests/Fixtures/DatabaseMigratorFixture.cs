using System;
using Chami.Db.Entities;
using Chami.Db.Migrations.Base;
using Chami.Db.Repositories;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Configuration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiTests.Fixtures;

public class DatabaseMigratorFixture : IDisposable
{
    public DatabaseMigratorFixture()
    {
        ConnectionString = "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureFluentMigrator(ConnectionString, typeof(ITestMigration)).BuildServiceProvider();

        var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateDown(0);
        migrationRunner.MigrateUp();

        EnvironmentRepository = new EnvironmentRepository(ConnectionString);
        EnvironmentDataAdapter = new EnvironmentDataAdapter(ConnectionString);
        SettingsDataAdapter = new SettingsDataAdapter(ConnectionString);
    }

    public EnvironmentDataAdapter EnvironmentDataAdapter { get; set; }
    public EnvironmentRepository EnvironmentRepository { get; set; }
    public SettingsDataAdapter SettingsDataAdapter { get; set; }
    internal string ConnectionString { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            EnvironmentDataAdapter = null;
            EnvironmentRepository = null;
            SettingsDataAdapter = null;
        }
    }
}