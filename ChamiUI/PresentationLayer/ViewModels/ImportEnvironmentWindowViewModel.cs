using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Utils;

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
        public ImportEnvironmentWindowViewModel(NewEnvironmentService newEnvironmentService) : base(newEnvironmentService)
        {
            NewEnvironments = new ObservableCollection<ImportEnvironmentViewModel>();
            SelectAllCommand = new AsyncCommand(ExecuteSelectAll, CanExecuteSelectAll);
            DeselectAllCommand = new AsyncCommand(ExecuteDeselectAll, CanExecuteDeselectAll);
            SaveCommand = new AsyncCommand<Window>(ExecuteSave, CanExecuteSave);
            NewEnvironments.CollectionChanged += NewEnvironmentsOnCollectionChanged;
            UpdatePropertyChanged();
        }

        private void NewEnvironmentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
            SaveCommand.RaiseCanExecuteChanged();
        }

        protected override async Task ExecuteSave(Window arg)
        {
            SetBusyState(true, "Saving data...");
            try
            {
                await Task.Run(() => _newEnvironmentService.SaveEnvironments(NewEnvironments));
            }
            finally
            {
                SetBusyState(false);
            }
           
            await CloseCommand.ExecuteAsync(arg);
        }

        private bool CanExecuteDeselectAll(object arg)
        {
            return NewEnvironments.Any(e => e.ShouldImport);
        }

        private async Task ExecuteDeselectAll()
        {
            foreach (var environment in NewEnvironments)
            {
                environment.ShouldImport = false;
            }

            SelectAllCommand.RaiseCanExecuteChanged();
            DeselectAllCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();

            await Task.CompletedTask;
        }

        private bool CanExecuteSelectAll(object arg)
        {
            return NewEnvironments.Any(e => e.ShouldImport == false);
        }

        private async Task ExecuteSelectAll()
        {
            foreach (var environment in NewEnvironments)
            {
                environment.ShouldImport = true;
            }

            DeselectAllCommand.RaiseCanExecuteChanged();
            SelectAllCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();

            await Task.CompletedTask;
        }

        public IAsyncCommand SelectAllCommand { get; }
        public IAsyncCommand DeselectAllCommand { get; }

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
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedEnvironmentName));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                SaveCommand.RaiseCanExecuteChanged();
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
                    EnvironmentViewModel environment = null; //DataAdapter.SaveEnvironment(environmentViewModel);
                    environments.Add(environment);
                }
            }

            return environments;
        }

        public void UpdatePropertyChanged()
        {
            OnPropertyChanged(nameof(NewEnvironments));
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
            SelectAllCommand.RaiseCanExecuteChanged();
            DeselectAllCommand.RaiseCanExecuteChanged();
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