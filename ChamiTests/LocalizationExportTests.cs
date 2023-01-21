using System.Globalization;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.Localization;
using Xunit;

namespace ChamiTests;


public class LocalizationExportTests
{
    [Fact]
    public void TestExport()
    {
        var exporter = new XliffLocalizationExporter();
        exporter.SetSourceType(typeof(ChamiUIStrings));
        exporter.TargetCulture = CultureInfo.GetCultureInfo("it-IT");
        exporter.Filename = "test.xlf";
        exporter.Export();
    }
}