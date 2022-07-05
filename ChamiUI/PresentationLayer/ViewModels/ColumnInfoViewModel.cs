using System.Windows.Data;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ColumnInfoViewModel : ViewModelBase
    {
        private bool _isVisible;
        private double _columnWidth;
        private Binding _binding;
        private int _ordinalPosition;
        private string _headerKey;
        private string _converter;
        private string _converterParameter;

        public string ConverterParameter
        {
            get => _converterParameter;
            set
            {
                _converterParameter = value;
                OnPropertyChanged(nameof(ConverterParameter));
            }
        }

        public string Header => ChamiUIStrings.ResourceManager.GetString(HeaderKey);

        public string Converter
        {
            get => _converter;
            set
            {
                _converter = value;
                OnPropertyChanged(nameof(Converter));
            }
        }

        public string HeaderKey
        {
            get => _headerKey;
            set
            {
                _headerKey = value;
                OnPropertyChanged(nameof(HeaderKey));
                OnPropertyChanged(nameof(Header));
            }
        }

        public int OrdinalPosition
        {
            get => _ordinalPosition;
            set
            {
                _ordinalPosition = value;
                OnPropertyChanged(nameof(OrdinalPosition));
            }
        }

        public Binding Binding
        {
            get => _binding;
            set
            {
                _binding = value;
                OnPropertyChanged(nameof(Binding));
            }
        }

        public double ColumnWidth
        {
            get => _columnWidth;
            set
            {
                _columnWidth = value;
                OnPropertyChanged(nameof(ColumnWidth));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
    }
}