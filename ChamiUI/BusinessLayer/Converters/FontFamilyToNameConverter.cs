using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;

namespace ChamiUI.BusinessLayer.Converters
{
    public class FontFamilyToNameConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}