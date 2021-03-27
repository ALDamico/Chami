using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class LoggingSettingsEditor : UserControl
    {
        public LoggingSettingsEditor()
        {
            InitializeComponent();
        }

        public LoggingSettingsEditor(LoggingSettingsViewModel loggingSettings)
        {
            _viewModel = loggingSettings;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private LoggingSettingsViewModel _viewModel;
    }
}