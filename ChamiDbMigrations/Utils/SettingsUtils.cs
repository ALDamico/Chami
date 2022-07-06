using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chami.Db.Entities;

namespace Chami.Db.Utils
{
    public static class SettingsUtils
    {
        private static IEnumerable<Setting> MakeSettingsInner(object settingsViewModel, Type settingType, PropertyInfo[] properties)
        {
            var settings = new List<Setting>();
            foreach (var property in properties)
            {
                
                Setting setting = new Setting();
                setting.Type = property.PropertyType.FullName;
                setting.Value = property.GetValue(settingsViewModel)?.ToString();
                setting.AssemblyName = settingType.Assembly.GetName().Name;
                setting.SettingName = property.Name;
                setting.ViewModelName = settingType.FullName;
                settings.Add(setting);
            }

            return settings;
        }

        public static IEnumerable<Setting> MakeSettings(object settingsViewModel, string[] propertiesToSave = null)
        {
            var settingType = settingsViewModel.GetType();
            var properties = GetPropertiesToSave(settingType, propertiesToSave);

            return MakeSettingsInner(settingsViewModel, settingType, properties);
        }

        private static PropertyInfo[] GetPropertiesToSave(Type settingType, string[] propertiesToSave = null)
        {
            var properties = settingType.GetProperties().Where(prop => prop.GetSetMethod(false) != null);

            if (propertiesToSave != null)
            {
                properties = properties.Where(p => propertiesToSave.Any(pts => pts == p.Name)).ToArray();
            }

            return properties.Where(p => p.GetCustomAttributes().All(prop => prop.GetType().Name != "NonPersistentSettingAttribute"))?.ToArray();
        }
    }
}