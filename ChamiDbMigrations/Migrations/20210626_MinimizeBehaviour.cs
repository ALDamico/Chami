using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    /// <summary>
    /// Adds support for customizing the behaviour of the application when minimizing the main window.
    /// It allows the user to either:
    /// Minimize the application to the taskbar
    /// Minimize the application to the tray. The user can then restore the window by double-clicking the Chami icon
    /// on the taskbar.
    /// </summary>
    [Migration(20210626)]
    public class MinimizeBehaviour : Migration
    {
        private static readonly Setting _minimizeBehaviourSetting;

        static MinimizeBehaviour()
        {
            _minimizeBehaviourSetting = new Setting()
            {
                SettingName = "MinimizationStrategy",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MinimizationBehaviourViewModel",
                PropertyName = "MinimizationBehaviour",
                Type = "ChamiUI.PresentationLayer.Minimizing.IMinimizationStrategy",
                Value = "ChamiUI.PresentationLayer.Minimizing.MinimizeToTaskbarStrategy",
               Converter = "ChamiUI.BusinessLayer.Converters.MinimizationStrategyConverter"
            };
        }
        
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName(typeof(Setting)))
                .Row(_minimizeBehaviourSetting);
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName(typeof(Setting)))
                .Row(_minimizeBehaviourSetting);
        }
    }
}