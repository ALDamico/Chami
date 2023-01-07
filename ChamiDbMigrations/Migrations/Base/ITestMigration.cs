using FluentMigrator;

namespace Chami.Db.Migrations.Base
{
    /// <summary>
    /// Marks a migration to be run applied only when tests are executed.
    /// </summary>
    [Tags("TEST")]
    public interface ITestMigration : IBaseMigration
    {
        
    }
}