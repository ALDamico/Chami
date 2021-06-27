using Chami.Db.Annotation;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
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
                Value = "Name#Ascending",
               Converter = "ChamiUI.BusinessLayer.Converters.SortDescriptionConverter"
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