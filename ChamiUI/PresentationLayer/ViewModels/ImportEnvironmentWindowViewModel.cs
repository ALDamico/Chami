using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chami.Db.Entities;

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
        /// The currently-selected environment in the environment listview.
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

        /// <summary>
        /// Determines if the save button is enabled, i.e. if they all pass validation.
        /// </summary>
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

        /// <summary>
        /// The name of the selected environment.
        /// </summary>
        public string SelectedEnvironmentName
        {
            get => SelectedEnvironment?.Name;
            set
            {
                SelectedEnvironment.Name = value;
                OnPropertyChanged(nameof(SelectedEnvironmentName));
            }
        }

        /// <summary>
        /// Converts all the viewmodels to save to <see cref="Environment"/> entities and saves them to the datastore.
        /// </summary>
        /// <returns></returns>
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