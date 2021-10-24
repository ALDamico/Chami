using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SafeVariableViewModel : SettingCategoryViewModelBase
    {
        public SafeVariableViewModel()
        {
            ForbiddenVariables = new ObservableCollection<EnvironmentVariableBlacklistViewModel>();
            _environmentDataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
        }

        public async Task LoadForbiddenVariables()
        {
            IsBusy = true;
            var forbiddenVariables = await _environmentDataAdapter.GetBlacklistedVariablesAsync();
            ForbiddenVariables.Clear();
            foreach (var variable in forbiddenVariables)
            {
                ForbiddenVariables.Add(variable);
            }

            IsBusy = false;
        }
        public ObservableCollection<EnvironmentVariableBlacklistViewModel> ForbiddenVariables { get; }

        private readonly EnvironmentDataAdapter _environmentDataAdapter;

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

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
    }
}