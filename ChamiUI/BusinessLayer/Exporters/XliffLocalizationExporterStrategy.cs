using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using ChamiUI.Utils;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;

namespace ChamiUI.BusinessLayer.Exporters;

public class XliffLocalizationExporterStrategy : ILocalizationExporterStrategy
{
    public void Export(IEnumerable<PropertyInfo> propertyInfos, ResourceManager resourceManager, string fileName, CultureInfo targetCulture)
    {
        var xliffDocument = new XliffDocument(targetCulture.IetfLanguageTag)
        {
            SourceLanguage = "en-US",
            TargetLanguage = targetCulture.GetBcp47LanguageCode()
        };

        var file = new File(fileName);
        var unit = new Unit("u1");
        file.Containers.Add(unit);
        xliffDocument.Files.Add(file);
        
        foreach (var propertyInfo in propertyInfos)
        {
            var obj = resourceManager.GetObject(propertyInfo.Name, CultureInfo.InvariantCulture);
            if (obj is not string str) continue;
            var segment = new Segment(propertyInfo.Name)
            {
                Source = new Source(str),
                Target = new Target
                {
                    Language = targetCulture.GetBcp47LanguageCode()
                }
            };

            unit.Resources.Add(segment);
        }
        
        var writer = new XliffWriter(new XliffWriterSettings {Indent = true, Detail = OutputDetail.Full});
        writer.Serialize(System.IO.File.OpenWrite(fileName), xliffDocument);
    }
}