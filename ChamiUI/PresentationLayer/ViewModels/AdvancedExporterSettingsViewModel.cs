namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterSettingsViewModel : GenericLabelViewModel
    {
        
        private int _maxLineLength;
        private int? _sessionMaxLineLength;
        private double _variableNameColumnWidth;
        private double _isMarkedColumnWidth;

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