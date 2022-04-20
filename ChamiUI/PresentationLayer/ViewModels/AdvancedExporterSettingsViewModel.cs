using System.Collections.ObjectModel;
using System.Windows.Media;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterSettingsViewModel : GenericLabelViewModel
    {
        public AdvancedExporterSettingsViewModel()
        {
            AvailableExporters = ExporterUtils.GetAvailableExporters();
        }
        public ObservableCollection<AdvancedExporterViewModel> AvailableExporters { get; }
        private int _maxLineLength;
        private int? _sessionMaxLineLength;
        private double _variableNameColumnWidth;
        private double _isMarkedColumnWidth;
        private Brush _previewBackgroundColor;
        private Brush _previewForegroundColor;
        private string _defaultExporter;
        private AdvancedExporterViewModel _selectedExporter;

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
    }
}