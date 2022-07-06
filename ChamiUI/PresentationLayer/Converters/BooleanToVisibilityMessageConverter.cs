using System;
using System.Globalization;
using System.Windows.Data;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Converters
{
    public class BooleanToVisibilityMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (booleanValue)
                {
                    return ChamiUIStrings.ColumnVisible;
                }

                return ChamiUIStrings.ColumnHidden;
            }

            if (value is string stringValue)
            {
                if (stringValue == ChamiUIStrings.ColumnVisible)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}