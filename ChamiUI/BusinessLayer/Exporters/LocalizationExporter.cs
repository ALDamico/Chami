using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using ChamiUI.BusinessLayer.Exceptions;
using ChamiUI.Utils;

namespace ChamiUI.BusinessLayer.Exporters;

public class LocalizationExporter : ILocalizationExporter
{
    public void Export(CultureInfo targetCulture)
    {
        if (string.IsNullOrWhiteSpace(Filename))
        {
            throw new ChamiInvalidConfigurationException("Filename can't be null");
        }

        if (Assembly == null)
        {
            throw new ChamiInvalidConfigurationException("Assembly can't be null");
        }
        
        if (LocalizationExporterStrategy == null)
        {
            throw new ChamiInvalidConfigurationException("LocalizationExporterStrategy can't be null");
        }
        
        var resource = Assembly.GetManifestResourceNames().SingleOrDefault(n => n.Replace(".resources", "") == DefaultDictionary);
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ChamiInvalidConfigurationException("Resources not found");
        }

        var f = resource.Replace(".resources", "");

        var resMan = new ResourceManager(f, Assembly);
        var properties = LocalizationUtils.GetLocalizableProperties(Assembly, DefaultDictionary);
        LocalizationExporterStrategy.Export(properties, resMan, Filename, targetCulture);
    }

    public string Filename { get; set; }
    public Assembly Assembly { get; set; }
    public string DefaultDictionary { get; set; }
    public ILocalizationExporterStrategy LocalizationExporterStrategy { get; set; }
}