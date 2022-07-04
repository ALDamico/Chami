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

            switch (value)
            {
                case null:
                    return null;
                case EnvironmentVariableHealthType healthType:
                {
                    var parameterString = (string) parameter;

                    return parameterString switch
                    {
                        "LongDescription" => healthType switch
                        {
                            EnvironmentVariableHealthType.Ok => ChamiUIStrings.EnvironmentHealthOkLongDescription,
                            EnvironmentVariableHealthType.MismatchedValue => ChamiUIStrings
                                .EnvironmentHealthMismatchedValueLongDescription,
                            EnvironmentVariableHealthType.MissingVariable => ChamiUIStrings
                                .EnvironmentHealthMissingValueLongDescription,
                            _ => string.Empty
                        },
                        _ => healthType switch
                        {
                            EnvironmentVariableHealthType.Ok => ChamiUIStrings.EnvironmentHealthOkShortDescription,
                            EnvironmentVariableHealthType.MismatchedValue => ChamiUIStrings
                                .EnvironmentHealthMismatchedValueShortDescription,
                            EnvironmentVariableHealthType.MissingVariable => ChamiUIStrings
                                .EnvironmentHealthMissingValueShortDescription,
                            _ => string.Empty
                        }
                    };
                }
                default:
                {
                    var message2 = string.Format(ChamiUIStrings.InvalidConversionOperationMessage, value.GetType());
                    throw new NotSupportedException(message2);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(EnvironmentVariableHealthType))
            {
                var message = string.Format(ChamiUIStrings.InvalidConversionOperationMessage, targetType.Name);
                throw new NotSupportedException(message);
            }

            if (value is not string str) return null;
            if (str == ChamiUIStrings.EnvironmentHealthOkShortDescription)
            {
                return EnvironmentVariableHealthType.Ok;
            }

            if (str == ChamiUIStrings.EnvironmentHealthMismatchedValueShortDescription)
            {
                return EnvironmentVariableHealthType.MismatchedValue;
            }

            if (str == ChamiUIStrings.EnvironmentHealthMissingValueShortDescription)
            {
                return EnvironmentVariableHealthType.MissingVariable;
            }

            return null;
        }
    }
}