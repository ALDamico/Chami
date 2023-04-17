using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ChamiUI.BusinessLayer.Exporters;

public interface ILocalizationExporterStrategy
{
    void Export(IEnumerable<PropertyInfo> propertyInfos, ResourceManager resourceManager, string fileName, CultureInfo targetCulture);
}