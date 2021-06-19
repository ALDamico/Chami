using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ImportEnvironmentWindowViewModel:NewEnvironmentViewModelBase
    {
        public ImportEnvironmentWindowViewModel():base()
        {
            NewEnvironments = new ObservableCollection<EnvironmentViewModel>();
        }
        public ObservableCollection<EnvironmentViewModel> NewEnvironments { get; }
        private EnvironmentViewModel _selectedEnvironment;

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