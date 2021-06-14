using System;
using System.Globalization;
using System.Windows.Data;

namespace ChamiUI.PresentationLayer.Converters
{
    public class BooleanToStringComparisonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StringComparison comparison)
            {
                if (comparison == StringComparison.InvariantCulture ||
                    comparison == StringComparison.CurrentCulture ||
                    comparison == StringComparison.Ordinal)
                {
                    return true;
                }
            }

            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (booleanValue)
                {
                    return StringComparison.InvariantCulture;
                }
            }

            return StringComparison.InvariantCultureIgnoreCase;
        }
    }
}