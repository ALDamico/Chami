using System.Globalization;
using System.Reflection;

namespace ChamiUI.BusinessLayer.Exporters;

public interface ILocalizationExporter
{
    void Export(CultureInfo targetCulture);
    string Filename { get; set; }
    Assembly Assembly { get; set; }
    string DefaultDictionary { get; set; }
}