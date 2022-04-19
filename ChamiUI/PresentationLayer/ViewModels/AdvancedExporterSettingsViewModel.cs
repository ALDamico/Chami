namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterSettingsViewModel : GenericLabelViewModel
    {
        private int _maxLineLength;
        private double _variableNameColumnWidth;
        private double _isMarkedColumnWidth;

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