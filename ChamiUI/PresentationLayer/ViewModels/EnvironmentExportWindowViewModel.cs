using Chami.Plugins.Contracts.ViewModels;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// ViewModel for the environments to show in the export window.
    /// </summary>
    public class EnvironmentExportWindowViewModel: ViewModelBase
    {
        private EnvironmentViewModel _environment;

        /// <summary>
        /// The environment.
        /// </summary>
        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
                OnPropertyChanged((nameof(NumVariables)));
                OnPropertyChanged(nameof(DisplayedName));
            }
        }

        /// <summary>
        /// The number of variables in the environment.
        /// </summary>
        public int NumVariables
        {
            get => Environment.EnvironmentVariables.Count;
        }

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