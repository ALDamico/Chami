using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Exporters;

public abstract class LocalizationExporterBase : ILocalizationExporter
{
    public abstract void Export();
    public abstract Task ExportAsync();
    
    protected ResourceManager GetResourceManagerFromSourceType()
    {
        return (ResourceManager)_sourceType.GetProperty("ResourceManager").GetMethod.Invoke(null, null);
    }
    
    protected Type _sourceType;
    public CultureInfo TargetCulture { get; set; }

    public void SetSourceType(Type sourceType)
    {
        _sourceType = sourceType;
    }
    
    protected IEnumerable<PropertyInfo> GetLocalizableProperties()
    {
        return _sourceType.GetProperties(BindingFlags.Static|BindingFlags.Public).Where(p => p.PropertyType != typeof(ResourceManager) && p.PropertyType != typeof(CultureInfo));
    }
}