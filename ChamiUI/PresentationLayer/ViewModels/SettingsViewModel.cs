using System.Collections.Generic;
using System.Windows.Controls;
using ChamiUI.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            _controls = new Dictionary<string, UserControl>();
            _controls["View"] = new ConsoleAppearanceEditor();
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
    }
}