using System.Windows;
using System.Windows.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class TabbedControlViewModel : ViewModelBase
    {
        private string _tabTitle;
        public string TabTitle
        {
            get => _tabTitle;
            set
            {
                _tabTitle = value;
                OnPropertyChanged(nameof(TabTitle));
            }
        }

        private bool? _isLocked;

        public bool? IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnPropertyChanged(nameof(IsLocked));
            }
        }

        private UserControl _control;
        public UserControl Control
        {
            get => _control;
            set
            {
                _control = value;
                OnPropertyChanged(nameof(Control));
            }
        }
    }
}