using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentJsonContractResolver: DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName != "Name")
            {
                return "Name";
            }

            return propertyName;
        }
    }
}