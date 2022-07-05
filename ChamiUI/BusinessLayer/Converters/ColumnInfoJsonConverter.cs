using System;
using System.Windows.Data;
using Newtonsoft.Json.Serialization;

using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ColumnInfoJsonConverter : JsonConverter<ColumnInfoViewModel>
    {
        private const double COLUMN_WIDTH_DEFAULT = 300;
        public override void WriteJson(JsonWriter writer, ColumnInfoViewModel value, JsonSerializer serializer)
        {
            writer.WriteValue(value.IsVisible);
            writer.WriteValue(value.ColumnWidth);
            writer.WriteValue(value.Binding.Path);
        }

        public override ColumnInfoViewModel ReadJson(JsonReader reader, Type objectType, ColumnInfoViewModel existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            ColumnInfoViewModel output = existingValue;
            if (existingValue == null)
            {
                output = new ColumnInfoViewModel();
            }

            JToken token = JObject.Load(reader);

            var isVisibleToken = token["IsVisible"];
            if (isVisibleToken != null)
            {
                var isVisible = isVisibleToken.Value<bool>();
                output.IsVisible = isVisible;
            }

            var columnWidthToken = token["ColumnWidth"];
            if (columnWidthToken != null)
            {
                var columnWidth = columnWidthToken.Value<double?>();

                if (columnWidth is null or 0 )
                {
                    columnWidth = COLUMN_WIDTH_DEFAULT;
                }

                output.ColumnWidth = columnWidth.Value;
            }

            var bindingToken = token["Binding"];
            if (bindingToken != null)
            {
                var bindingTokenValue = bindingToken.Value<string>();
                if (!string.IsNullOrWhiteSpace(bindingTokenValue))
                {
                    output.Binding = new Binding(bindingTokenValue);
                }
            }

            int ordinalPosition = 0;
            var ordinalPositionToken = token["OrdinalPosition"];
            var ordinalPositionValue = ordinalPositionToken?.Value<int?>();
            if (ordinalPositionValue != null)
            {
                ordinalPosition = ordinalPositionValue.Value;
            }

            output.OrdinalPosition = ordinalPosition;
            
            
            
            return output;
        }
    }
}