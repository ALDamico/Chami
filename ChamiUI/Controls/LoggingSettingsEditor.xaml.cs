using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    /// <summary>
    /// Control for managing the logging settings of the Chami application.
    /// </summary>
    public partial class LoggingSettingsEditor
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
            DataContext = loggingSettings;
            InitializeComponent();
        }
    }
}