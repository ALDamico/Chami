using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using Karambolo.PO;

namespace ChamiUI.BusinessLayer.Exporters;

public class GetTextLocalizationExporterStrategy : ILocalizationExporterStrategy
{
    public void Export(IEnumerable<PropertyInfo> propertyInfos, ResourceManager resourceManager, string fileName, CultureInfo targetCulture)
    {
        var poCatalog = new POCatalog();
        poCatalog.Encoding = Encoding.UTF8.WebName;
        var generator = new POGenerator();

        foreach (var propertyInfo in propertyInfos)
        {
            var obj = resourceManager.GetObject(propertyInfo.Name);
            if (obj is not string str)
            {
                continue;
            }

            var key = new POKey(propertyInfo.Name);
            var entry = new POSingularEntry(key)
            {
                Translation = str
            };
            poCatalog.Add(entry);
        }

        var writer = File.OpenWrite(fileName);
        generator.Generate(writer, poCatalog);
    }
}