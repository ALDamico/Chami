using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Services;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateWindowViewModel : ViewModelBase
    {
        public MassUpdateWindowViewModel(MassUpdateService massUpdateService)
        {
            _massUpdateService = massUpdateService;
            KnownVariables = new ObservableCollection<string>();
            UpdateStrategies = new ObservableCollection<MassUpdateStrategyViewModel>();
            InitUpdateStrategies();
            Environments = new ObservableCollection<EnvironmentViewModel>();
            NewValue = "";
            ExecuteMassUpdateCommand = new AsyncCommand(ExecuteUpdate, CanExecute);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            ExecuteMassUpdateCommand?.RaiseCanExecuteChanged();
            base.OnPropertyChanged(propertyName);
        }

        private readonly MassUpdateService _massUpdateService;

        private void InitUpdateStrategies()
        {
            var availableStrategies = _massUpdateService.GetAvailableMassUpdateStrategies();
            foreach (var updateStrategy in availableStrategies)
            {
                UpdateStrategies.Add(updateStrategy);
            }

            SelectedUpdateStrategy = UpdateStrategies.FirstOrDefault();
        }

        private string _variableToUpdate;
        private string _newValue;
        private bool _createVariableIfNotExists;
        private MassUpdateStrategyViewModel _selectedUpdateStrategy;

        public string NewValue
        {
            get => _newValue;
            set
            {
                _newValue = value;
                OnPropertyChanged();
            }
        }

        public MassUpdateStrategyViewModel SelectedUpdateStrategy
        {
            get => _selectedUpdateStrategy;
            set
            {
                _selectedUpdateStrategy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EnvironmentListBoxEnabled));
            }
        }

        public bool CreateVariableIfNotExists
        {
            get => _createVariableIfNotExists;
            set
            {
                _createVariableIfNotExists = value;
                OnPropertyChanged();
            }
        }

        public string VariableToUpdate
        {
            get => _variableToUpdate;
            set
            {
                _variableToUpdate = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand ExecuteMassUpdateCommand { get; }

        private async Task ExecuteUpdate()
        {
            await _massUpdateService.ExecuteUpdate(SelectedUpdateStrategy, VariableToUpdate, NewValue,
                SelectedEnvironments,
                CreateVariableIfNotExists);
        }

        public bool CanExecute(object param)
        {
            if (string.IsNullOrWhiteSpace(VariableToUpdate))
            {
                return false;
            }

            if (!SelectedUpdateStrategy.Equals(MassUpdateStrategyViewModel.DefaultUpdateStrategy) &&
               !SelectedEnvironments.Any())
            {
                return false;
            }

            return true;
        }

        public ObservableCollection<string> KnownVariables { get; }
        public ObservableCollection<MassUpdateStrategyViewModel> UpdateStrategies { get; }
        public ObservableCollection<EnvironmentViewModel> Environments { get; }

        private IEnumerable<EnvironmentViewModel> SelectedEnvironments => Environments.Where(e => e.IsSelected);

        public async Task LoadDataAsync()
        {
            var tasks = _massUpdateService.GetLoadDataTask();
            await Task.WhenAll(tasks);
            Task<IEnumerable<EnvironmentViewModel>> environmentDataTask =
                (Task<IEnumerable<EnvironmentViewModel>>)tasks[0];
            var variableDataTask = (Task<IEnumerable<string>>)tasks[1];

            foreach (var element in environmentDataTask.Result)
            {
                Environments.Add(element);
            }

            foreach (var element in variableDataTask.Result)
            {
                KnownVariables.Add(element);
            }
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

        public Action HandleSelectionChanged => ExecuteMassUpdateCommand.RaiseCanExecuteChanged;
        private int? _selectedIndex;

        public int? SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }


        public void SelectAllEnvironments()
        {
            foreach (var environmentViewModel in Environments)
            {
                environmentViewModel.IsSelected = true;
            }
        }

        public void DeselectAllEnvironments()
        {
            foreach (var environmentViewModel in Environments)
            {
                environmentViewModel.IsSelected = false;
            }
        }


        public bool ShouldShowWarningMessageBox()
        {
            return string.IsNullOrWhiteSpace(NewValue);
        }
    }
}