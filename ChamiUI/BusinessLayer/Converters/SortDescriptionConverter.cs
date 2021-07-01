using System;
using System.ComponentModel;
using System.Globalization;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts a <see cref="Setting"/> entity to a <see cref="SortDescription"/>.
    /// </summary>
    public class SortDescriptionConverter
    {
        /// <summary>
        /// Converts from a <see cref="Setting"/> to a <see cref="SortDescription"/>. If no direction can be determined, assumes the sorting is ascending.
        /// </summary>
        /// <param name="setting">The setting to convert from.</param>
        /// <returns>A <see cref="SortDescription"/> object.</returns>
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

        /// <summary>
        /// Converts a <see cref="SortDescription"/> to a <see cref="Setting"/>.
        /// </summary>
        /// <param name="sortDescription">The <see cref="SortDescription"/> to convert from.</param>
        /// <returns>A <see cref="Setting"/> entity that can be saved in the datastore.</returns>
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

        /// <summary>
        /// Gets the value to update the Value property of a <see cref="Setting"/> object.
        /// </summary>
        /// <param name="sortDescription">The <see cref="SortDescription"/> to convert from.</param>
        /// <returns>A string containing the <see cref="SortDescription"/> representation that Chami can use to save it as a <see cref="Setting"/>.</returns>
        public string GetSettingValue(SortDescription sortDescription)
        {
            var fieldName = sortDescription.PropertyName;
            var direction = sortDescription.Direction.ToString();
            var representationValue = string.Join("#", new string[] {fieldName, direction});
            return representationValue;
        }
    }
}