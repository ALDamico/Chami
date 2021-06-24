using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Chami.Db.Entities;
using Chami.Db.Repositories;

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
                        $"The requested property was not found in type {viewModel.GetType().ToString()}");
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
                            BindingFlags.Default, null, args: new[] { setting.Value }, null, null);
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
                                    propertyValue = methodInfo.Invoke(unwrappedConverter, new[] { setting });
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

                settingSetMethod.Invoke(pInfo.GetValue(viewModel), new[] { propertyValue });

                pInfo.SetValue(viewModel, pInfo.GetValue(viewModel));
            }


            return viewModel;
        }

        private SettingsRepository _repository;

        public SettingsViewModel GetSettings()
        {
            var settingsList = _repository.GetSettings();
            return ToViewModel(settingsList);
        }

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
                        var conversionSuccessful = double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tmp);
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
                var propertiesToSave = propertyInfo.PropertyType.GetProperties();
                foreach (var property in propertiesToSave)
                {
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


    }
}