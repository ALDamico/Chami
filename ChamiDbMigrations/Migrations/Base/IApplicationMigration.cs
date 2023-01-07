using FluentMigrator;

namespace Chami.Db.Migrations.Base
{
    /// <summary>
    /// Marks a migration to be run when the application is started
    /// </summary>
    [Tags("APP")]
    public interface IApplicationMigration : IBaseMigration
    {
        
    }
}