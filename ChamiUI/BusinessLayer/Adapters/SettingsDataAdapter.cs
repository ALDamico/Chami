using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        /// <summary>
        /// Converts the <see cref="Setting"/> entities to a suitable <see cref="SettingsViewModel"/> that the presentation layer can use by performing various conversions.
        /// The algorithm for performing these conversions is as follows:
        /// <list type="number">
        ///     <item>
        ///         At first, it attempts to perform a series of trivial conversions based on the Type property of the <see cref="Setting"/> entity. If no such conversion can be performed, an <see cref="InvalidCastException"/> is thrown.
        ///         The following types are supported:
        ///         <list type="bullet">
        ///             <item>string</item>
        ///             <item>System.Boolean or bool</item>
        ///             <item>System.Int32 or int</item>
        ///             <item>System.Double or double</item>
        ///         </list>
        ///         Other types could be added, but are probably not needed.
        ///     </item>
        ///     <item>
        ///         If the previous attempt failed, tries to invoke the constructor of the target type by using the CreateInstance of <see cref="Activator"/> and passing the Value property of the setting.
        ///         In practice, this means that the algorithm attempts to invoke a constructor that takes a string as a parameter.
        ///         If such a constructor doesn't exist, a <see cref="MissingMethodException"/> is raised.
        ///     </item>
        ///     <item>
        ///         If the previous step failed, it tries to instantiate the Converter specified in the Setting by invoking its parameterless constructor.
        ///         If that succeeds, it invokes its Convert method.
        ///     </item>
        ///     <item>
        ///         If all fails, the conversion is considered impossible.
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="settings">The full list of settings available to the Chami application.</param>
        /// <returns>A <see cref="SettingsViewModel"/> object for use by the presentation layer.</returns>
        /// <exception cref="NullReferenceException">The method isn't able to find a property whose name corresponds to Setting.PropertyName, or inside this it cannot find a property named after Setting.SettingName.</exception>
        /// <exception cref="InvalidOperationException">The value conversion was successful, but the target property lacks a publicly-accessible setter.</exception>
        public SettingsViewModel ToViewModel(IEnumerable<Setting> settings)
        {
            var viewModel = new SettingsViewModel();
            foreach (var setting in settings)
            {
                var pInfo = viewModel.GetType().GetProperty(setting.PropertyName);
                if (pInfo == null)
                {
                    throw new InvalidDataException(
                        $"The requested property was not found in type {viewModel.GetType()}");
                }

                var settingPInfo = pInfo.PropertyType.GetProperty(setting.SettingName);
                if (settingPInfo == null)
                {
                    throw new InvalidDataException($"The requested property was not found in object {pInfo.Name}");
                }

                if (settingPInfo.GetCustomAttribute<NonPersistentSettingAttribute>() != null)
                {
                    continue;
                }

                var targetTypeName = setting.Type;
                var propertyValue = AttemptConversion(targetTypeName, setting);

                var prop = settingPInfo.GetValue(pInfo.GetValue(viewModel));

                if (prop is IList<ColumnInfo> list)
                {
                    var columnInfos = _repository.GetColumnInfoBySettingName(setting.SettingName);
                    foreach (var columnInfo in columnInfos)
                    {
                        list.Add(columnInfo);
                    }
                    
                }
                if (prop is not IList)
                {
                    var settingSetMethod = settingPInfo.GetSetMethod();
                    if (settingSetMethod == null)
                    {
                        throw new InvalidOperationException(
                            $"The requested property {settingPInfo.Name} has no publicly-accessible setter!");
                    }

                    settingSetMethod.Invoke(pInfo.GetValue(viewModel), new[] {propertyValue});
                }


                pInfo.SetValue(viewModel, pInfo.GetValue(viewModel));
            }


            return viewModel;
        }

        private static object ConvertToEnumType(string assemblyName, string targetTypeName, Setting setting)
        {
            object propertyValue = null;
            var defaultValue = Activator.CreateInstance(assemblyName, targetTypeName);
            if (defaultValue != null)
            {
                var unwrappedValue = defaultValue.Unwrap();
                Type unwrappedValueType = unwrappedValue.GetType();
                propertyValue = Enum.Parse(unwrappedValueType, setting.Value);
            }

            return propertyValue;
        }

        private object AttemptConversion(string targetTypeName, Setting setting)
        {
            object propertyValue = null;
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
                    propertyValue = ConvertViaUnwrapping(setting);
                }
                catch (TypeLoadException)
                {
                    propertyValue = ConvertViaUnwrapping(setting);
                }
            }

            return propertyValue;
        }

        private static object ConvertViaUnwrapping(Setting setting)
        {
            try
            {
                var converter = Activator.CreateInstance(null, setting.Converter);
                object propertyValue = null;
                var unwrappedConverter = converter.Unwrap();
                if (unwrappedConverter != null)
                {
                    var methodInfo = unwrappedConverter.GetType().GetMethod("Convert");
                    if (methodInfo != null)
                    {
                        propertyValue = methodInfo.Invoke(unwrappedConverter, new object[] {setting});
                    }
                }

                return propertyValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private readonly SettingsRepository _repository;

        /// <summary>
        /// Retrieves settings from the datastore and converts them to an instance of <see cref="SettingsViewModel"/>
        /// </summary>
        /// <returns>A <see cref="SettingsViewModel"/> containing all application settings.</returns>
        public SettingsViewModel GetSettings()
        {
            var settingsList = _repository.GetSettings();
            return ToViewModel(settingsList);
        }

        /// <summary>
        /// Private method that handles nullable value types.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> object from a Regex</param>
        /// <returns>The type name, if found, otherwise an empty <see cref="Match"/>.</returns>
        private string NullableValueTypeMatcher(Match match)
        {
            if (match.Success)
            {
                if (match.Groups.Count > 1)
                {
                    return match.Groups[1].Value;
                }
            }

            return Match.Empty.Value;
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

            if (typeName.ToLowerInvariant().StartsWith("nullable"))
            {
                typeName = Regex.Replace(typeName, @"nullable\<(\w+)\>", NullableValueTypeMatcher, RegexOptions.IgnoreCase);
            }
            
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

        /// <summary>
        /// Converts every property in the <see cref="SettingsViewModel"/> to a <see cref="Setting"/> entity and updates it in the datastore.
        /// Not every property is guaranteed to be saved. For performance reasons, some classes are marked as "explicit save only" and are only updated when strictly necessary. Others are marked as "non persistent" and they're never going to be saved.
        /// </summary>
        /// <param name="settings">The <see cref="SettingsViewModel"/> to convert and save.</param>
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

                if (propertyInfo.GetValue(settings) is IList<ColumnInfo> list)
                {
                    foreach (var columnInfo in list)
                    {
                        _repository.UpdateColumnInfo(columnInfo);
                    }
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

                    valueString ??= "NULL";

                    _repository.UpdateSetting(propertyInfo.Name, propertyName, valueString);
                }
            }
        }

        /// <summary>
        /// Saves all settings related to the state of <see cref="MainWindow"/> to the datastore.
        /// This is used to allow hiding or closing the main window and then restoring its state at a later time.
        /// </summary>
        /// <param name="settings">The modified application settings.</param>
        public void SaveMainWindowState(SettingsViewModel settings)
        {
            var mainWinSettings = settings.MainWindowBehaviourSettings;
            _repository.UpdateSetting( nameof(MainWindowSavedBehaviourViewModel), "IsCaseSensitiveSearch",
                mainWinSettings.IsCaseSensitiveSearch.ToString());
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.Height),
                mainWinSettings.Height.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.Width),
                mainWinSettings.Width.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.XPosition),
                mainWinSettings.XPosition.ToString(CultureInfo.InvariantCulture));
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.YPosition),
                mainWinSettings.YPosition.ToString(CultureInfo.InvariantCulture));
            var filterStrategyConverter = new FilterStrategyConverter();
            var filterStrategyValue = filterStrategyConverter.GetSettingValue(mainWinSettings.SearchPath);
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.SearchPath),
                filterStrategyValue);
            var sortDescriptionConverter = new SortDescriptionConverter();
            var sortDescriptionValue = sortDescriptionConverter.GetSettingValue(mainWinSettings.SortDescription);
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.SortDescription),
                sortDescriptionValue);
            _repository.UpdateSetting(nameof(MainWindowSavedBehaviourViewModel), nameof(MainWindowSavedBehaviourViewModel.WindowState),
                ((int) mainWinSettings.WindowState).ToString());
        }

        public async Task<EnvironmentVariableBlacklistViewModel> SaveBlacklistedVariableAsync(
            EnvironmentVariableBlacklistViewModel variable)
        {
            var converter = new EnvironmentVariableBlacklistConverter();
            var entity = converter.From(variable);

            await _repository.UpsertBlacklistedVariableAsync(entity);

            return converter.To(entity);
        }

        public async Task<IEnumerable<EnvironmentVariableBlacklistViewModel>> SaveBlacklistedVariableListAsync(
            IEnumerable<EnvironmentVariableBlacklistViewModel> variables)
        {
            var tasks = new List<Task<EnvironmentVariableBlacklist>>();
            var output = new List<EnvironmentVariableBlacklistViewModel>();
            var converter = new EnvironmentVariableBlacklistConverter();
            foreach (var variable in variables)
            {
                var converted = converter.From(variable);
                var task = _repository.UpsertBlacklistedVariableAsync(converted);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                output.Add(converter.To(task.Result));
            }

            return output;
        }

        public async Task SaveColumnInfoAsync(IEnumerable<ColumnInfoViewModel> columnInfoViewModels)
        {
            var tasks = new List<Task>();
            var converter = new ColumnInfoConverter();
            foreach (var columnInfoViewModel in columnInfoViewModels)
            {
                var columnInfo = converter.From(columnInfoViewModel);
                tasks.Add(_repository.UpdateColumnInfoAsync(columnInfo));
            }

            await Task.WhenAll(tasks);
        }

        public void SaveFontSize(double fontSize)
        {
            _repository.UpdateSetting(nameof(ConsoleAppearanceViewModel), nameof(ConsoleAppearanceViewModel.FontSize), fontSize.ToString(CultureInfo.InvariantCulture));
        }

        public List<Setting> GetSettingsList()
        {
            return _repository.GetSettings().ToList();
        }
    }
}
