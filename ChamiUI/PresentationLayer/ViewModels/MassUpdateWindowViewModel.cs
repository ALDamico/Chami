using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateWindowViewModel : ViewModelBase
    {
        public MassUpdateWindowViewModel(MassUpdateService massUpdateService)
        {
            _massUpdateService = massUpdateService;
            ExecuteMassUpdateCommand = new AsyncCommand(ExecuteUpdate, CanExecute);
            CloseWindowCommand = new AsyncCommand<Window>(ExecuteCloseWindow);
            SelectAllCommand = new AsyncCommand(ExecuteSelectAll, CanExecuteSelectAll);
            DeselectAllCommand = new AsyncCommand(ExecuteDeselectAll, CanExecuteSelectAll);
            KnownVariables = new ObservableCollection<string>();
            UpdateStrategies = new ObservableCollection<MassUpdateStrategyViewModel>();
            InitUpdateStrategies();
            Environments = new ObservableCollection<EnvironmentViewModel>();
            NewValue = "";
            
        }

        private async Task ExecuteDeselectAll()
        {
            DeselectAllEnvironments();
            await Task.CompletedTask;
        }

        private bool CanExecuteSelectAll(object arg)
        {
            return EnvironmentListBoxEnabled;
        }

        private async Task ExecuteSelectAll()
        {
            SelectAllEnvironments();
            await Task.CompletedTask;
        }

        private async Task ExecuteCloseWindow(Window arg)
        {
            arg.Close();
            await Task.CompletedTask;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            ExecuteMassUpdateCommand?.RaiseCanExecuteChanged();
            DeselectAllCommand?.RaiseCanExecuteChanged();
            SelectAllCommand?.RaiseCanExecuteChanged();
            base.OnPropertyChanged(propertyName);
        }

        private readonly MassUpdateService _massUpdateService;
        private string _variableToUpdate;
        private string _newValue;
        private MassUpdateStrategyViewModel _selectedUpdateStrategy;

        private void InitUpdateStrategies()
        {
            var availableStrategies = _massUpdateService.GetAvailableMassUpdateStrategies();
            foreach (var updateStrategy in availableStrategies)
            {
                UpdateStrategies.Add(updateStrategy);
            }

            SelectedUpdateStrategy = UpdateStrategies.FirstOrDefault();
        }

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
                if (value.CreateIfNotExistsEnabled && SelectedUpdateStrategy is {CreateIfNotExistsEnabled: true})
                {
                    value.CreateIfNotExists = SelectedUpdateStrategy.CreateIfNotExists;
                }
                _selectedUpdateStrategy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EnvironmentListBoxEnabled));
                SelectAllCommand.RaiseCanExecuteChanged();
                DeselectAllCommand.RaiseCanExecuteChanged();

                if (value is {EnvironmentListBoxEnabled: false})
                {
                    DeselectAllEnvironments();
                }
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
        public AsyncCommand<Window> CloseWindowCommand { get; }
        public IAsyncCommand SelectAllCommand { get; }
        public IAsyncCommand DeselectAllCommand { get; }

        private async Task ExecuteUpdate()
        {
            if (ShouldShowWarningMessageBox())
            {
                ShowMessageBox(_ => { ExecuteUpdateInner().GetAwaiter().GetResult(); }, 
                    string.Format(ChamiUIStrings.ConfirmMassUpdateWithEmptyValueMessageBoxMessage, VariableToUpdate),
                    ChamiUIStrings.ConfirmMassUpdateWithEmptyValueMessageBoxCaption, MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.No);
                return;
            }

            await ExecuteUpdateInner();
        }

        private async Task ExecuteUpdateInner()
        {
            await _massUpdateService.ExecuteUpdate(SelectedUpdateStrategy, VariableToUpdate, NewValue,
                SelectedEnvironments,
                SelectedUpdateStrategy.CreateIfNotExists);
        }


        private bool CanExecute(object param)
        {
            if (string.IsNullOrWhiteSpace(VariableToUpdate))
            {
                return false;
            }

            return SelectedUpdateStrategy.Equals(MassUpdateStrategyViewModel.DefaultUpdateStrategy) ||
                   SelectedEnvironments.Any();
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
                (Task<IEnumerable<EnvironmentViewModel>>) tasks[0];
            var variableDataTask = (Task<IEnumerable<string>>) tasks[1];

            foreach (var element in environmentDataTask.Result)
            {
                Environments.Add(element);
            }

            foreach (var element in variableDataTask.Result)
            {
                KnownVariables.Add(element);
            }
        }

        public bool EnvironmentListBoxEnabled =>
            !SelectedUpdateStrategy.Equals(MassUpdateStrategyViewModel.DefaultUpdateStrategy);

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

        private void SelectAllEnvironments()
        {
            foreach (var environmentViewModel in Environments)
            {
                environmentViewModel.IsSelected = true;
            }
        }

        private void DeselectAllEnvironments()
        {
            if (Environments == null)
            {
                return;
            }
            foreach (var environmentViewModel in Environments)
            {
                environmentViewModel.IsSelected = false;
            }
        }
        
        private bool ShouldShowWarningMessageBox()
        {
            return string.IsNullOrWhiteSpace(NewValue);
        }
    }
}