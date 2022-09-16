using System;
using System.Globalization;
using System.Windows.Data;

namespace ChamiUI.PresentationLayer.Converters;

public class StringToUriConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ExecuteConversion(value, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ExecuteConversion(value, targetType, parameter, culture);
    }

    private object ExecuteConversion(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        if (targetType.Equals(typeof(Uri)) || value.GetType().Equals(typeof(string)))
        {
            return ConvertStringToUri((string) value, parameter, culture);
        }

        return ConvertUriToString((Uri) value, parameter, culture);
    }

    private object ConvertStringToUri(string value, object parameter, CultureInfo cultureInfo)
    {
        var stringValue = value?.ToString();

        return stringValue != null ? new Uri(stringValue) : null;
    }

    private object ConvertUriToString(Uri value, object parameter, CultureInfo cultureInfo)
    {
        var uriValue = value;
        if (uriValue == null)
        {
            return null;
        }

        return uriValue.ToString();
    }
}