using System.Globalization;
using System.IO;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.Localization;
using Xunit;

namespace ChamiTests;


public class LocalizationExportTests
{
    [Fact]
    public void TestExport()
    {
        string fileName = "test.xlf";
        var exporter = new XliffLocalizationExporter();
        exporter.SetSourceType(typeof(ChamiUIStrings));
        exporter.TargetCulture = CultureInfo.GetCultureInfo("it-IT");
        exporter.SourceCulture = CultureInfo.GetCultureInfo("en-US");
        exporter.Filename = fileName;
        exporter.Export();
        var fileExists = File.Exists(fileName);
        Assert.True(fileExists);
    }

    [Fact]
    public void TextPOExport()
    {
        string fileName = "test.po";
        var exporter = new GetTextLocalizationExporter();
        exporter.Filename = fileName;
        exporter.TargetCulture = CultureInfo.GetCultureInfo("pl-PL");
        exporter.SourceCulture = CultureInfo.GetCultureInfo("en-US");
        exporter.SetSourceType(typeof(ChamiUIStrings));
        exporter.Export();
        var fileExists = File.Exists(fileName);
        Assert.True(fileExists);
    }
}