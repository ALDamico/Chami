using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            NewEnvironments = new ObservableCollection<ImportEnvironmentViewModel>();
            UpdatePropertyChanged();
        }

        /// <summary>
        /// The list of new environments to import.
        /// </summary>
        public ObservableCollection<ImportEnvironmentViewModel> NewEnvironments { get; }

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
                if (NewEnvironments.All(e => !e.ShouldImport))
                {
                    return false;
                }
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
                OnPropertyChanged();
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
                if (environmentViewModel.ShouldImport)
                {
                    EnvironmentViewModel environment = null;//DataAdapter.SaveEnvironment(environmentViewModel);
                    environments.Add(environment);
                }
            }

            return environments;
        }

        public void UpdatePropertyChanged()
        {
           OnPropertyChanged(nameof(NewEnvironments));
           OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }

        public void SelectAllEnvironments()
        {
            foreach (var environment in NewEnvironments)
            {
                environment.ShouldImport = true;
            }
        }

        public void DeselectAllEnvironments()
        {
            foreach (var environment in NewEnvironments)
            {
                environment.ShouldImport = false;
            }
        }
    }
}