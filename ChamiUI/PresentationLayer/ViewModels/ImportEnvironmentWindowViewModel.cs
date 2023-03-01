using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Validators;

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
            ValidationRules = new List<ValidationRule>();
            NewEnvironments = new ObservableCollection<ImportEnvironmentViewModel>();
            UpdatePropertyChanged();
            ValidationRules.Add(new EnvironmentVariableNameNotNullValidationRule()
                {ValidationStep = ValidationStep.UpdatedValue});
            ValidationRules.Add(new EnvironmentVariableNameLengthValidationRule()
                {MaxLength = 2047, ValidationStep = ValidationStep.UpdatedValue});
            ValidationRules.Add(new EnvironmentVariableNameNoNumberFirstCharacterValidationRule()
                {ValidationStep = ValidationStep.UpdatedValue});
            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = SelectedEnvironment?.EnvironmentVariables;
            ValidationRules.Add(new EnvironmentVariableNameUniqueValidationRule()
                {ValidationStep = ValidationStep.CommittedValue, EnvironmentVariables = collectionViewSource});
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

                (ValidationRules.FirstOrDefault(r => r is EnvironmentVariableNameUniqueValidationRule) as
                        EnvironmentVariableNameUniqueValidationRule).EnvironmentVariables.Source =
                    value.EnvironmentVariables;
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
                if (environmentViewModel.ShouldImport)
                {
                    var environment = DataAdapter.SaveEnvironment(environmentViewModel);
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