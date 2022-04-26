using System.Collections.ObjectModel;
using System.Windows.Media;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterSettingsViewModel : GenericLabelViewModel
    {
        public AdvancedExporterSettingsViewModel()
        {
            AvailableExporters = ExporterUtils.GetAvailableExporters();
            FontFamilies = SettingsUtils.GetInstalledFonts();
        }
        public ObservableCollection<AdvancedExporterViewModel> AvailableExporters { get; }
        public ObservableCollection<FontFamily> FontFamilies { get; }
        private int _maxLineLength;
        private int? _sessionMaxLineLength;
        private double _variableNameColumnWidth;
        private double _isMarkedColumnWidth;
        private Brush _previewBackgroundColor;
        private Brush _previewForegroundColor;
        private string _defaultExporter;
        private AdvancedExporterViewModel _selectedExporter;
        private FontFamily _selectedFont;
        private double _fontSize;

        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        public FontFamily SelectedFont
        {
            get => _selectedFont;
            set
            {
                _selectedFont = value;
                OnPropertyChanged(nameof(SelectedFont));
            }
        }

        public AdvancedExporterViewModel SelectedExporter
        {
            get => _selectedExporter;
            set
            {
                _selectedExporter = value;
                OnPropertyChanged(nameof(SelectedExporter));
            }
        }

        public string DefaultExporter
        {
            get => _defaultExporter;
            set
            {
                _defaultExporter = value;
                OnPropertyChanged(nameof(DefaultExporter));
            }
        }

        public Brush PreviewForegroundColor
        {
            get => _previewForegroundColor;
            set
            {
                _previewForegroundColor = value;
                OnPropertyChanged(nameof(PreviewForegroundColor));
            }
        }

        public Brush PreviewBackgroundColor
        {
            get => _previewBackgroundColor;
            set
            {
                _previewBackgroundColor = value;
                OnPropertyChanged(nameof(PreviewBackgroundColor));
            }
        }

        [NonPersistentSetting]
        public int? SessionMaxLineLength
        {
            get => _sessionMaxLineLength;
            set
            {
                _sessionMaxLineLength = value;
                OnPropertyChanged(nameof(SessionMaxLineLength));
            }
        }

        public double IsMarkedColumnWidth
        {
            get => _isMarkedColumnWidth;
            set
            {
                _isMarkedColumnWidth = value;
                OnPropertyChanged(nameof(IsMarkedColumnWidth));
            }
        }

        public double VariableNameColumnWidth
        {
            get => _variableNameColumnWidth;
            set
            {
                _variableNameColumnWidth = value;
                OnPropertyChanged(nameof(VariableNameColumnWidth));
            }
        }

        public int MaxLineLength
        {
            get => _maxLineLength;
            set
            {
                _maxLineLength = value;
                OnPropertyChanged(nameof(MaxLineLength));
            }
        }

        public void ChangeForegroundColor(Color newColor)
        {
            PreviewForegroundColor = new SolidColorBrush(newColor);
        }

        public void ChangeBackgroundColor(Color newColor)
        {
            PreviewBackgroundColor = new SolidColorBrush(newColor);
        }
    }
}