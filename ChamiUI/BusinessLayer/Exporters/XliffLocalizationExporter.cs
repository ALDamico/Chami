using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;
using File = System.IO.File;

namespace ChamiUI.BusinessLayer.Exporters;

public class XliffLocalizationExporter : LocalizationExporterBase
{
    

    private string _fileId;
    private string _unitId;

    public string FileId
    {
        get => _fileId ?? Assembly.GetExecutingAssembly().GetName().Name;
        set => _fileId = value;
    }
    
    public string UnitId
    {
        get => _unitId ?? Assembly.GetExecutingAssembly().GetName().Name;
        set => _unitId = value;
    }

    public override bool CanExport()
    {
        if (string.IsNullOrWhiteSpace(Filename))
        {
            return false;
        }

        return base.CanExport();
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
        var file = new global::Localization.Xliff.OM.Core.File(FileId);
        var unit = new Unit(UnitId);
        file.Containers.Add(unit);
        
        xliffDocument.Files.Add(file);
        foreach (var propertyName in properties.Select(p => p.Name))
        {
            var segment = new Segment();

            SetSegmentContent(resourceManager, segment, propertyName, SourceCulture);
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