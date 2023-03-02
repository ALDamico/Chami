using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// ViewModel for the environments to show in the export window.
    /// </summary>
    public class EnvironmentExportWindowViewModel: ViewModelBase
    {
        private EnvironmentViewModel _environment;
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                Environment.IsSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The environment.
        /// </summary>
        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged();
                OnPropertyChanged((nameof(NumVariables)));
                OnPropertyChanged(nameof(DisplayedName));
            }
        }

        /// <summary>
        /// The number of variables in the environment.
        /// </summary>
        public int NumVariables => Environment.EnvironmentVariables.Count;

        /// <summary>
        /// The name to display in the listview for this environment.
        /// </summary>
        public string DisplayedName
        {
            get
            {
                var environmentName = Environment.Name;
                var numVariables = NumVariables;

                return string.Format(ChamiUIStrings.EnvironmentExportWindowViewModelDisplayedName, environmentName, numVariables);
            }
        }
    }
}