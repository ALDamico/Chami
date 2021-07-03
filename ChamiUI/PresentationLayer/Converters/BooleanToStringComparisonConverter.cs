using System;
using System.Globalization;
using System.Windows.Data;

namespace ChamiUI.PresentationLayer.Converters
{
    /// <summary>
    /// Converts between boolean values and <see cref="StringComparison"/> objects. If case-sensitive
    /// <see cref="StringComparison"/> evaluate to true, whereas case-insentive objects evaluate to false.
    /// </summary>
    public class BooleanToStringComparisonConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="StringComparison"/> to a boolean value.
        /// </summary>
        /// <param name="value">The <see cref="StringComparison"/> to convert.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts boolean values to a <see cref="StringComparison"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused</param>
        /// <param name="culture">Unused.</param>
        /// <returns></returns>
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