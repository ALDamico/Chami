using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ChamiUI.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        public SettingsWindowViewModel(SettingsViewModel settings) 
        {
            _controls = new Dictionary<string, UserControl>();
            _controls["View"] = new ConsoleAppearanceEditor(settings.ConsoleAppearanceSettings);
            _controls["Logging"] = new LoggingSettingsEditor(settings.LoggingSettings);
            _controls["Safety"] = new SafeVariableEditor(settings.SafeVariableSettings);
            DisplayedControl = _controls.Values.FirstOrDefault();
        }

        public SettingsWindowViewModel()
        {
            _controls = new Dictionary<string, UserControl>();
            _controls["View"] = new ConsoleAppearanceEditor();
            _controls["Logging"] = new LoggingSettingsEditor();
            _controls["Safety"] = new SafeVariableEditor();
            DisplayedControl = _controls.Values.FirstOrDefault();
        }

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