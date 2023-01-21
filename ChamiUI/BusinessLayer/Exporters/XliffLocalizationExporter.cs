using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using ChamiUI.Localization;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Modules.FormatStyle;
using Localization.Xliff.OM.Modules.Metadata;
using Localization.Xliff.OM.Modules.Validation;
using Localization.Xliff.OM.Serialization;
using File = System.IO.File;

namespace ChamiUI.BusinessLayer.Exporters;

public class XliffLocalizationExporter : LocalizationExporterBase
{
    public string Filename { get; set; }

    public bool CanExport()
    {
        if (string.IsNullOrWhiteSpace(Filename))
        {
            return false;
        }

        if (_sourceType == null)
        {
            return false;
        }

        return true;
    }

    private void SetSegmentContent(ResourceManager resourceManager, Segment segment, string propertyName,
        CultureInfo targetCulture, bool setTarget = false)
    {
        var value = resourceManager.GetObject(propertyName, targetCulture);
        if (setTarget)
        {
            segment.Target = new Target();
            segment.Target.Language = targetCulture.Name;
            segment.Target.Text.Add(new PlainText((string)value));
        }
        else
        {
            segment.Source = new Source();
            segment.Source.Language = targetCulture.Name;
            segment.Source.Text.Add(new PlainText((string)value));
        }
    }
    public override void Export()
    {
        if (!CanExport())
        {
            throw new InvalidOperationException();
        }
        var xliffWriter = new XliffWriter();
        using var stream = File.OpenWrite(Filename);
        var xliffDocument = new XliffDocument(CultureInfo.GetCultureInfo("en-us").Name);
        xliffDocument.Space = Preservation.Default;
        xliffDocument.TargetLanguage = TargetCulture.Name;
        var properties = GetLocalizableProperties();
        var resourceManager = GetResourceManagerFromSourceType();
        var file = new global::Localization.Xliff.OM.Core.File("Chami.Localization");
        var unit = new Unit("chamiLocalization");
        file.Containers.Add(unit);
        
        xliffDocument.Files.Add(file);
        foreach (var propertyName in properties.Select(p => p.Name))
        {
            var segment = new Segment();

            SetSegmentContent(resourceManager, segment, propertyName, CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
            SetSegmentContent(resourceManager, segment, propertyName, TargetCulture, true);

            unit.Resources.Add(segment);
        }
        xliffWriter.Serialize(stream, xliffDocument);
    }

    public override async Task ExportAsync()
    {
        await Task.Run(Export);
    }
}