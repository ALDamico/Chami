using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the import window.
    /// </summary>
    public class ImportEnvironmentWindowViewModel : NewEnvironmentViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="ImportEnvironmentWindowViewModel"/> object and initializes its properties.
        /// </summary>
        public ImportEnvironmentWindowViewModel()
        {
            NewEnvironments = new ObservableCollection<EnvironmentViewModel>();
        }

        /// <summary>
        /// The list of new environments to import.
        /// </summary>
        public ObservableCollection<EnvironmentViewModel> NewEnvironments { get; }
        private EnvironmentViewModel _selectedEnvironment;

        /// <summary>
        /// 
        /// </summary>
        public EnvironmentViewModel SelectedEnvironment
        {
            get => _selectedEnvironment;
            set
            {
                _selectedEnvironment = value;
                OnPropertyChanged(nameof(SelectedEnvironment));
                OnPropertyChanged(nameof(SelectedEnvironmentName));
            }
        }

        public override bool IsSaveButtonEnabled
        {
            get
            {
                foreach (var environment in NewEnvironments)
                {
                    if (!Validator.Validate(environment).IsValid)
                    {
                        return false;
                    }
                }

                return true;
            }
        }


        public string SelectedEnvironmentName
        {
            get => SelectedEnvironment.Name;
            set
            {
                SelectedEnvironment.Name = value;
                OnPropertyChanged(nameof(SelectedEnvironmentName));
            }
        }

        public IEnumerable<EnvironmentViewModel> SaveEnvironments()
        {
            var environments = new List<EnvironmentViewModel>();
            foreach (var environmentViewModel in NewEnvironments)
            {
                var environment = DataAdapter.SaveEnvironment(environmentViewModel);
                environments.Add(environment);
            }

            return environments;
        }
    }
}