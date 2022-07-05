using System;
using System.Windows.Data;
using Chami.Db.Entities;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ColumnInfoConverter : IConverter<ColumnInfo, ColumnInfoViewModel>
    {
        

        public ColumnInfo From(ColumnInfoViewModel model)
        {
            return new ColumnInfo
            {
                Binding = model.Binding.Path.Path,
                Header = model.HeaderKey,
                ColumnWidth = model.ColumnWidth,
                IsVisible = model.IsVisible,
                OrdinalPosition = model.OrdinalPosition,
                Converter = model.Converter,
                ConverterParameter = model.ConverterParameter
            };
        }

        public ColumnInfoViewModel To(ColumnInfo entity)
        {
            var binding = new Binding(entity.Binding);

            if (entity.Converter != null)
            {
                var converterType = Type.GetType(entity.Converter);
                if (converterType == null)
                {
                    throw new InvalidOperationException($"Converter type ${entity.Converter} not found!");
                }
                binding.Converter = Activator.CreateInstance(converterType) as IValueConverter;
                if (entity.ConverterParameter != null)
                {
                    binding.ConverterParameter = entity.ConverterParameter;
                }
            }
            
            
            return new ColumnInfoViewModel
            {
                Binding = binding,
                HeaderKey = entity.Header,
                ColumnWidth = entity.ColumnWidth,
                IsVisible = entity.IsVisible,
                OrdinalPosition = entity.OrdinalPosition ?? default,
                Converter = entity.Converter
            };
        }
    }
}