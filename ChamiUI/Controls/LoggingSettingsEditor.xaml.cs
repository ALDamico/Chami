using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;

namespace ChamiUI.Controls
{
    /// <summary>
    /// Control for managing the logging settings of the Chami application.
    /// </summary>
    public partial class LoggingSettingsEditor : UserControl
    {
        /// <summary>
        /// Constructs a new <see cref="LoggingSettingsEditor"/> object.
        /// </summary>
        public LoggingSettingsEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructs a new <see cref="LoggingSettingsEditor"/> and initializes its viewmodel.
        /// </summary>
        /// <param name="loggingSettings">The starting <see cref="LoggingSettingsViewModel"/>.</param>
        public LoggingSettingsEditor(LoggingSettingsViewModel loggingSettings)
        {
            _viewModel = loggingSettings;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private LoggingSettingsViewModel _viewModel;
    }
}