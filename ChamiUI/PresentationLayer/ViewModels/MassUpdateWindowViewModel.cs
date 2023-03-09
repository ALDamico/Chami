using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Localization;
using ChamiUI.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// ViewModel for the mass update window.
    /// </summary>
    public class MassUpdateWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="MassUpdateWindowViewModel"/> object
        /// </summary>
        /// <param name="massUpdateService"></param>
        public MassUpdateWindowViewModel(MassUpdateService massUpdateService)
        {
            _massUpdateService = massUpdateService;
            ExecuteMassUpdateCommand = new AsyncCommand(ExecuteUpdate, CanExecuteMassUpdate);
            CloseCommand = new AsyncCommand<Window>(ExecuteCloseWindow);
            SelectAllCommand = new AsyncCommand(ExecuteSelectAll, CanExecuteSelectAll);
            DeselectAllCommand = new AsyncCommand(ExecuteDeselectAll, CanExecuteSelectAll);
            KnownVariables = new ObservableCollection<string>();
            UpdateStrategies = new ObservableCollection<MassUpdateStrategyViewModel>();
            InitUpdateStrategies();
            Environments = new ObservableCollection<EnvironmentViewModel>();
            NewValue = "";
            LoadDataAsync().Await();
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

        /// <summary>
        /// The update strategy to use when updating the selected environment(s).
        /// </summary>
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

        /// <summary>
        /// The name of the variable to update.
        /// </summary>
        public string VariableToUpdate
        {
            get => _variableToUpdate;
            set
            {
                _variableToUpdate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A command that handles the mass update of environments.
        /// </summary>
        public IAsyncCommand ExecuteMassUpdateCommand { get; }
        
        /// <summary>
        /// A command to select all environments.
        /// </summary>
        public IAsyncCommand SelectAllCommand { get; }
        /// <summary>
        /// A command to deselect all environments.
        /// </summary>
        public IAsyncCommand DeselectAllCommand { get; }

        /// <summary>
        /// Executes the mass update asynchronously.
        /// </summary>
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


        /// <summary>
        /// Determines whether the <see cref="ExecuteMassUpdateCommand"/> can be triggered or not.
        /// </summary>
        /// <param name="param">Not used.</param>
        /// <returns>True if the mass update can take place, otherwise false.</returns>
        private bool CanExecuteMassUpdate(object param)
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