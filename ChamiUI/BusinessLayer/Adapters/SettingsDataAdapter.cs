using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.DataLayer.Repositories;

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
                    var parts = setting.Type.Split(".");
                    var typeName = parts.Last();
                    //var assemblyName = setting.Type.Replace("." + typeName, "");
                    var assemblyName = setting.AssemblyName;
                    try
                    {
                        var objectWrapper = Activator.CreateInstance(assemblyName, setting.Type, false,
                            BindingFlags.Default, null, args: new[] {setting.Value}, null, null);
                        if (objectWrapper != null)
                        {
                            propertyValue = objectWrapper.Unwrap();
                        }
                    }
                    catch (MissingMethodException ex)
                    {
                        var converter = Activator.CreateInstance(nameof(ChamiUI), setting.Converter);
                        if (converter != null)
                        {
                            var unwrappedConverter = converter.Unwrap();
                            var methodInfo = unwrappedConverter.GetType().GetMethod("Convert");
                            propertyValue = methodInfo.Invoke(unwrappedConverter, new[] {setting});
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
    }
}