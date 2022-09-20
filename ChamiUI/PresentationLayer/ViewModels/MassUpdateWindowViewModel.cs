using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChamiDbMigrations.Migrations;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateWindowViewModel : ViewModelBase
    {
        public MassUpdateWindowViewModel()
        {
            KnownVariables = new ObservableCollection<string>();
            UpdateStrategies = new ObservableCollection<MassUpdateStrategyViewModel>();
            InitUpdateStrategies();
            Environments = new ObservableCollection<EnvironmentViewModel>();
            SelectedEnvironments = new ObservableCollection<EnvironmentViewModel>();
            _environmentDataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
            NewValue = "";
        }

        private void InitUpdateStrategies()
        {
            var updateAllStrategy = MassUpdateStrategyViewModel.DefaultUpdateStrategy;
            UpdateStrategies.Add(updateAllStrategy);

            var updateSelectedStrategy = new MassUpdateStrategyViewModel()
            {
                Name = ChamiUIStrings.MassUpdateStrategyName_UpdateSelected,
                CreateIfNotExistsEnabled = true,
                EnvironmentListBoxEnabled = true
            };
            
            UpdateStrategies.Add(updateSelectedStrategy);
            var createOnlyStrategy = new MassUpdateStrategyViewModel()
            {
                Name = ChamiUIStrings.MassUpdateStrategyName_CreateOnly,
                CreateIfNotExistsEnabled = false,
                CreateIfNotExists = true,
                EnvironmentListBoxEnabled = false
            };
            UpdateStrategies.Add(createOnlyStrategy);
            SelectedUpdateStrategy = updateAllStrategy;
        }

        private string _variableToUpdate;
        private string _newValue;
        private bool _createVariableIfNotExists;
        private MassUpdateStrategyViewModel _selectedUpdateStrategy;
        private readonly EnvironmentDataAdapter _environmentDataAdapter;

        public string NewValue
        {
            get => _newValue;
            set
            {
                _newValue = value;
                OnPropertyChanged(nameof(NewValue));
            }
        }

        public MassUpdateStrategyViewModel SelectedUpdateStrategy
        {
            get => _selectedUpdateStrategy;
            set
            {
                _selectedUpdateStrategy = value;
                OnPropertyChanged(nameof(SelectedUpdateStrategy));
                OnPropertyChanged(nameof(EnvironmentListBoxEnabled));
            }
        }

        public bool CreateVariableIfNotExists
        {
            get => _createVariableIfNotExists;
            set
            {
                _createVariableIfNotExists = value;
                OnPropertyChanged(nameof(CreateVariableIfNotExists));
            }
        }

        public string VariableToUpdate
        {
            get => _variableToUpdate;
            set
            {
                _variableToUpdate = value;
                OnPropertyChanged(nameof(VariableToUpdate));
                OnPropertyChanged(nameof(ExecuteButtonEnabled));
            }
        }

        public bool ExecuteButtonEnabled
        {
            get
            {
                if (string.IsNullOrWhiteSpace(VariableToUpdate))
                {
                    return false;
                }

                if (!SelectedUpdateStrategy.Equals(MassUpdateStrategyViewModel.DefaultUpdateStrategy) &&
                    SelectedEnvironments.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        public ObservableCollection<string> KnownVariables { get; }
        public ObservableCollection<MassUpdateStrategyViewModel> UpdateStrategies { get; }
        public ObservableCollection<EnvironmentViewModel> Environments { get; }
        public ObservableCollection<EnvironmentViewModel> SelectedEnvironments { get; }

        public async Task LoadDataAsync()
        {
            var environmentDataTask = Task.Run(_environmentDataAdapter.GetEnvironments);
            var variableDataTask = _environmentDataAdapter.GetVariableNamesAsync();

            var tasks = new List<Task>();
            tasks.Add(environmentDataTask);
            tasks.Add(variableDataTask);
            await Task.WhenAll(tasks);


            foreach (var element in environmentDataTask.Result)
            {
                Environments.Add(element);
            }

            foreach (var element in variableDataTask.Result)
            {
                KnownVariables.Add(element);
            }
        }

        public void HandleSelectionChanged(IList eAddedItems, IList eRemovedItems)
        {
            foreach (EnvironmentViewModel removedItem in eRemovedItems)
            {
                SelectedEnvironments.Remove(removedItem);
            }

            foreach (EnvironmentViewModel addedItem in eAddedItems)
            {
                SelectedEnvironments.Add(addedItem);
            }

            OnPropertyChanged(nameof(ExecuteButtonEnabled));
        }

        public bool EnvironmentListBoxEnabled
        {
            get
            {
                if (SelectedUpdateStrategy.Equals(MassUpdateStrategyViewModel.DefaultUpdateStrategy))
                {
                    return false;
                }

                return true;
            }
        }

        public void SelectAllEnvironments()
        {
            SelectedEnvironments.Clear();

            foreach (var environment in Environments)
            {
                SelectedEnvironments.Add(environment);
            }
        }

        public void DeselectAllEnvironments()
        {
            SelectedEnvironments.Clear();
        }

        public async Task ExecuteUpdate()
        {
            var massUpdateStrategy = MassUpdateStrategyFactory.GetMassUpdateStrategyByViewModel(SelectedUpdateStrategy,
                VariableToUpdate, NewValue, SelectedEnvironments, CreateVariableIfNotExists);
            await massUpdateStrategy.ExecuteUpdateAsync(_environmentDataAdapter);
            if (CreateVariableIfNotExists)
            {
                foreach (var environment in SelectedEnvironments)
                {
                    environment.EnvironmentVariables.Add(new EnvironmentVariableViewModel()
                        {Environment = environment, Name = VariableToUpdate, Value = NewValue});
                    _environmentDataAdapter.SaveEnvironment(environment);
                }
            }
        }

        public bool ShouldShowWarningMessageBox()
        {
            return string.IsNullOrWhiteSpace(NewValue);
        }
    }
}