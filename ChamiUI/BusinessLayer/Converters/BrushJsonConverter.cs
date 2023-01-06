using System;
using System.Text.Json;
using System.Windows.Media;
using Chami.Db.Entities;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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
        return _brushConverter.Convert(value.ToString());
    }
}