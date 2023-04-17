using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChamiUI.Utils;

public static class LocalizationUtils
{
    public static IEnumerable<PropertyInfo> GetLocalizableProperties(Assembly assembly, string defaultDictionary)
    {
        var resource = assembly.GetManifestResourceNames().SingleOrDefault(n => n.Replace(".resources", "") == defaultDictionary);
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new Exception();
        }
        
        var f = resource.Replace(".resources", "");

        var properties = assembly.GetType(f)?.GetProperties() ?? Array.Empty<PropertyInfo>();

        return properties;
    }
}