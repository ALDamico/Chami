using System;
using System.ComponentModel;
using System.Globalization;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public class SortDescriptionConverter
    {
        public SortDescription Convert(Setting setting)
        {
            var values = setting.Value.Split("#");
            var fieldName = values[0];
            string direction = null;
            if (values.Length > 1)
            {
                direction = values[1];
            }

            if (direction == null)
            {
                direction = ListSortDirection.Ascending.ToString();
            }
            
            return new SortDescription(fieldName,
                (ListSortDirection) Enum.Parse(typeof(ListSortDirection), direction));
        }

        public Setting ConvertBack(SortDescription sortDescription)
        {
            var representationValue = GetSettingValue(sortDescription);
            return new Setting()
            {
                SettingName = "SortDescription",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "SortDescription",
                Value = representationValue,
                AssemblyName = "WindowsBase"
            };
        }

        public string GetSettingValue(SortDescription sortDescription)
        {
            var fieldName = sortDescription.PropertyName;
            var direction = sortDescription.Direction.ToString();
            var representationValue = string.Join("#", new string[] {fieldName, direction});
            return representationValue;
        }
    }
}