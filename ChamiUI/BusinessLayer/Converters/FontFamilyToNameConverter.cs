using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Extracts the font name from a <see cref="FontFamily"/> object.
    /// </summary>
    public class FontFamilyToNameConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">The value to convert from. It can either be a <see cref="FontFamily"/> object or a string.</param>
        /// <param name="targetType">Unused</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FontFamily family)
            {
                return SplitFamilyName(family.Source);
            }

            if (value is string str)
            {
                return SplitFamilyName(str);
            }

            return null;
        }

        /// <summary>
        /// Takes the font family name and extracts the font name from it (the part after the first # character)
        /// </summary>
        /// <param name="name">The font famiily name.</param>
        /// <returns>A string containing the UI-friendly font name.</returns>
        private string SplitFamilyName(string name)
        {
            var nameParts = Regex.Split(name, "#");
            if (nameParts.Length > 1)
            {
                return nameParts[1];
            }

            if (nameParts.Length == 1)
            {
                return name;
            }

            return null;
        }

        /// <summary>
        /// Always throws a <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}