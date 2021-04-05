using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        public SettingsWindowViewModel()
        {
            var connectionString = App.GetConnectionString();
            _dataAdapter = new SettingsDataAdapter(connectionString);
            Settings = _dataAdapter.GetSettings();
            _controls = new Dictionary<string, UserControl>();
            _controls["View"] = new ConsoleAppearanceEditor(Settings.ConsoleAppearanceSettings);
            _controls["Logging"] = new LoggingSettingsEditor(Settings.LoggingSettings);
            _controls["Safety"] = new SafeVariableEditor(Settings.SafeVariableSettings);
            _controls["Detector"] = new ApplicationDetectorControl(Settings.WatchedApplicationSettings);
            DisplayedControl = _controls.Values.FirstOrDefault();
        }

        public void SaveSettings()
        {
            _dataAdapter.SaveSettings(Settings);
        }

        private SettingsDataAdapter _dataAdapter;
        private Dictionary<string, UserControl> _controls;

        public void ChangeControl(string name)
        {
            var controlExists = _controls.TryGetValue(name, out var control);
            if (controlExists)
            {
                DisplayedControl = control;
            }
            else
            {
                DisplayedControl = null;
            }
        }

        private UserControl _displayedControl;

        public UserControl DisplayedControl
        {
            get => _displayedControl;
            set
            {
                _displayedControl = value;
                OnPropertyChanged(nameof(DisplayedControl));
            }
        }

        private SettingsViewModel _settingsViewModel;

        public SettingsViewModel Settings
        {
            get => _settingsViewModel;
            set
            {
                _settingsViewModel = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

    }
}