using System;
using System.Globalization;
using System.Windows.Data;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Converters
{
    public class EnvironmentVariableHealthTypeToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                var message = string.Format(ChamiUIStrings.InvalidConversionOperationMessage, targetType.Name);
                throw new NotSupportedException(message);
            }

            if (value == null)
            {
                return null;
            }

            if (value is EnvironmentVariableHealthType healthType)
            {
                if (parameter == "ShortDescription")
                {
                    switch (healthType)
                    {
                        case EnvironmentVariableHealthType.Ok:
                            return ChamiUIStrings.EnvironmentHealthOkShortDescription;
                        case EnvironmentVariableHealthType.MismatchedValue:
                            return ChamiUIStrings.EnvironmentHealthMismatchedValueShortDescription;
                        case EnvironmentVariableHealthType.MissingVariable:
                            return ChamiUIStrings.EnvironmentHealthMissingValueShortDescription;
                    }
                }
                else if (parameter == "LongDescription")
                {
                    switch (healthType)
                    {
                        case EnvironmentVariableHealthType.Ok:
                            return ChamiUIStrings.EnvironmentHealthOkLongDescription;
                        case EnvironmentVariableHealthType.MismatchedValue:
                            return ChamiUIStrings.EnvironmentHealthMismatchedValueLongDescription;
                        case EnvironmentVariableHealthType.MissingVariable:
                            return ChamiUIStrings.EnvironmentHealthMissingValueLongDescription;
                    }
                }

                return null;
            }

            var message2 = string.Format(ChamiUIStrings.InvalidConversionOperationMessage, value.GetType());
            throw new NotSupportedException(message2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(EnvironmentVariableHealthType))
            {
                var message = string.Format(ChamiUIStrings.InvalidConversionOperationMessage, targetType.Name);
                throw new NotSupportedException(message);
            }
            if (value is string str)
            {
                if (value == ChamiUIStrings.EnvironmentHealthOkShortDescription)
                {
                    return EnvironmentVariableHealthType.Ok;
                }
                else if (value == ChamiUIStrings.EnvironmentHealthMismatchedValueShortDescription)
                {
                    return EnvironmentVariableHealthType.MismatchedValue;
                }
                else if (value == ChamiUIStrings.EnvironmentHealthMissingValueShortDescription)
                {
                    return EnvironmentVariableHealthType.MissingVariable;
                }
            }

            return null;
        }
    }
}