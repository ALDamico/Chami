using System;
using System.Collections.Generic;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.BusinessLayer.Adapters
{
    public class SettingsDataAdapter
    {
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

                var targetTypeName = setting.Type;
                object propertyValue = GetBoxedConvertedObject(targetTypeName, setting.Value);

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
                    var conversionSuccessful = double.TryParse(value, out double tmp);
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