using System.Globalization;

namespace ChamiUI.Utils;

public static class CultureInfoExtensions
{
    public static string GetBcp47LanguageCode(this CultureInfo cultureInfo)
    {
        return cultureInfo.IetfLanguageTag.Replace("_", "-");
    }
}