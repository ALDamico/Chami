using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karambolo.PO;

namespace ChamiUI.BusinessLayer.Exporters;

public class GetTextLocalizationExporter : LocalizationExporterBase
{
    public override void Export()
    {
        if (!CanExport())
        {
            throw new InvalidOperationException();
        }
        POCatalog poCatalog = new POCatalog();
        poCatalog.Encoding = "UTF-8";
        POGenerator poGenerator = new POGenerator();
        poCatalog.Language = TargetCulture.Name;
        var localizableProperties =  GetLocalizableProperties();
        var resourceManager = GetResourceManagerFromSourceType();
        var fileStream = File.OpenWrite(Filename);
        var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
        foreach (var property in localizableProperties.Select(p => p.Name))
        {
            var poEntry = new POSingularEntry(new POKey(property));
            poEntry.Translation = (string) resourceManager.GetObject(property);
            poCatalog.Add(poEntry);
        }

        poGenerator.Generate(streamWriter, poCatalog);
    }

    public override async Task ExportAsync()
    {
        await Task.Run(Export);
    }
}