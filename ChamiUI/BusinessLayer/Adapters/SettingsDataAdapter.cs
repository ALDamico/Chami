using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.BusinessLayer.Adapters
{
    public class SettingsDataAdapter
    {
        public SettingsDataAdapter(string connectionString)
        {
            _repository = new SettingsRepository(connectionString);
        }

        public SettingsViewModel ToViewModel(IEnumerable<Setting> settings)
        {
            var viewModel = new SettingsViewModel();
            foreach (var setting in settings)
            {
                var pInfo = viewModel.GetType().GetProperty(setting.PropertyName);
                if (pInfo == null)
                {
                    throw new NullReferenceException(
                        $"The requested property was not found in type {viewModel.GetType()}");
                }

                var settingPInfo = pInfo.PropertyType.GetProperty(setting.SettingName);
                if (settingPInfo == null)
                {
                    throw new NullReferenceException($"The requested property was not found in object {pInfo.Name}");
                }

                object propertyValue = null;
                var targetTypeName = setting.Type;
                try
                {
                    propertyValue = GetBoxedConvertedObject(targetTypeName, setting.Value);
                }
                catch (InvalidCastException)
                {
                    var assemblyName = setting.AssemblyName;
                    try
                    {
                        var objectWrapper = Activator.CreateInstance(assemblyName, setting.Type, false,
                            BindingFlags.Default, null, args: new object[] {setting.Value}, null, null);
                        if (objectWrapper != null)
                        {
                            propertyValue = objectWrapper.Unwrap();
                        }
                    }
                    catch (MissingMethodException)
                    {
                        var converter = Activator.CreateInstance(nameof(ChamiUI), setting.Converter);
                        if (converter != null)
                        {
                            var unwrappedConverter = converter.Unwrap();
                            if (unwrappedConverter != null)
                            {
                                var methodInfo = unwrappedConverter.GetType().GetMethod("Convert");
                                if (methodInfo != null)
                                {
                                    propertyValue = methodInfo.Invoke(unwrappedConverter, new object[] {setting});
                                }
                            }
                        }
                    }
                }

                var settingSetMethod = settingPInfo.GetSetMethod();
                if (settingSetMethod == null)
                {
                    throw new InvalidOperationException(
                        $"The requested property {settingPInfo.Name} has no publicly-accessible setter!");
                }

                settingSetMethod.Invoke(pInfo.GetValue(viewModel), new[] {propertyValue});

                pInfo.SetValue(viewModel, pInfo.GetValue(viewModel));
            }


            return viewModel;
        }

        private readonly SettingsRepository _repository;

        /// <summary>
        /// Retrieves settings from the datastore.
        /// </summary>
        /// <returns>A <see cref="SettingsViewModel"/> containing all application settings.</returns>
        public SettingsViewModel GetSettings()
        {
            var settingsList = _repository.GetSettings();
            return ToViewModel(settingsList);
        }

        /// <summary>
        /// Tries to perform some basic conversions on the values retrieved from the datastore.
        /// </summary>
        /// <param name="typeName">Name of the target type</param>
        /// <param name="value">Value to convert</param>
        /// <returns>A converted object, usually of a "primitive" type.</returns>
        /// <exception cref="InvalidCastException">If no cast can be performed, throws an InvalidCastException</exception>
        private object GetBoxedConvertedObject(string typeName, string value)
        {
            object propertyValue = null;
            switch (typeName)
            {
                case "System.Boolean":
                case "bool":
                    propertyValue = value.StringToBool();
                    break;
                case "System.String":
                case "string":
                    propertyValue = value;
                    break;
                case "System.Int32":
                case "int":
                {
                    var conversionSuccessful = int.TryParse(value, out int tmp);
                    if (conversionSuccessful)
                    {
                        propertyValue = tmp;
                    }

                    break;
                }
                case "System.Double":
                case "double":
                {
                    var conversionSuccessful =
                        double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tmp);
                    if (conversionSuccessful)
                    {
                        propertyValue = tmp;
                    }

                    break;
                }
                default: throw new InvalidCastException("Requested type not supported!");
            }

            return propertyValue;
        }

        public void SaveSettings(SettingsViewModel settings)
        {
            var propertyInfos = settings.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                // We don't want to save some settings every time, because they depend on something other than the 
                // settings window.
                // For example, we don't want to update the settings related to the main window's state, because 
                // those are updated explicitly by a dedicated method
                var isExplicitSaveOnlyAttribute =
                    propertyInfo.PropertyType.GetCustomAttribute<ExplicitSaveOnlyAttribute>();
                if (isExplicitSaveOnlyAttribute is {IsExplicitSaveOnly: true})
                {
                    continue;
                }

                var propertiesToSave = propertyInfo.PropertyType.GetProperties();
                foreach (var property in propertiesToSave)
                {
                    var isNonPersistent = property.GetCustomAttribute<NonPersistentSettingAttribute>();
                    if (isNonPersistent is {IsNonPersistent: true})
                    {
                        continue;
                    }
                    string valueString = null;
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(propertyInfo.GetValue(settings));

                    if (propertyValue != null)
                    {
                        valueString = propertyValue.ToString();
                    }

                    _repository.UpdateSetting(propertyName, valueString);
                }
            }
        }

        /// <summary>
        /// Saves all settings related to the state of <see cref="MainWindow"/> to the datastore.
        /// </summary>
        /// <param name="settings">The modified application settings.</param>
        public void SaveMainWindowState(SettingsViewModel settings)
        {
            var mainWinSettings = settings.MainWindowBehaviourSettings;
            _repository.UpdateSetting("IsCaseSensitiveSearch",
                mainWinSettings.IsCaseSensitiveSearch.ToString());
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.Height),
                mainWinSettings.Height.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.Width),
                mainWinSettings.Width.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.XPosition),
                mainWinSettings.XPosition.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.YPosition),
                mainWinSettings.YPosition.ToString(CultureInfo.InvariantCulture));
            var filterStrategyConverter = new FilterStrategyConverter();
            var filterStrategyValue = filterStrategyConverter.GetSettingValue(mainWinSettings.SearchPath);
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.SearchPath),
                filterStrategyValue);
            var sortDescriptionConverter = new SortDescriptionConverter();
            var sortDescriptionValue = sortDescriptionConverter.GetSettingValue(mainWinSettings.SortDescription);
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel.SortDescription),
                sortDescriptionValue);
        }
    }
}