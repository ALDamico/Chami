using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SafeVariableViewModel : SettingCategoryViewModelBase
    {
        public SafeVariableViewModel()
        {
            ForbiddenVariables = new ObservableCollection<EnvironmentVariableViewModel>();
        }
        public ObservableCollection<EnvironmentVariableViewModel> ForbiddenVariables { get; }

        private bool _enableSafeVars;

        public bool EnableSafeVars
        {
            get => _enableSafeVars;
            set
            {
                _enableSafeVars = value;
                OnPropertyChanged(nameof(EnableSafeVars));
            }
        }
    }
}