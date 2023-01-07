using System;
using System.Windows.Media;
using Newtonsoft.Json;

namespace ChamiUI.BusinessLayer.Converters;

public class BrushJsonConverter : JsonConverter<Brush>
{
   

    private readonly BrushConverter _brushConverter = new BrushConverter();
    public override void WriteJson(JsonWriter writer, Brush value, JsonSerializer serializer)
    {
        writer.WriteToken(JsonToken.String, value.ToString());
    }

    public override Brush ReadJson(JsonReader reader, Type objectType, Brush existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var value = reader.Value;
        return _brushConverter.ConvertString(value.ToString());
    }
}