using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using Chami.Plugins.Contracts.ViewModels;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Exporters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the export window.
    /// </summary>
    public class ExportWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="ExportWindowViewModel"/> and sets its default values.
        /// </summary>
        public ExportWindowViewModel()
        {
            ExportAll = true;
            ExportSelected = false;
            Environments = new ObservableCollection<EnvironmentExportWindowViewModel>();
            SelectedEnvironments = new ObservableCollection<EnvironmentExportWindowViewModel>();
            var connectionString = App.GetConnectionString();
            _repository = new EnvironmentRepository(connectionString);
        }

        private EnvironmentRepository _repository;

        /// <summary>
        /// Constructs a new <see cref="ExportWindowViewModel"/> object and adds an initial set of environment
        /// variables.
        /// </summary>
        /// <param name="environments">The starting environments for the window.</param>
        public ExportWindowViewModel(ICollection<EnvironmentViewModel> environments) : this()
        {
            var converter = new EnvironmentExportConverter();
            foreach (var environment in environments)
            {
                var converted = converter.From(environment);
                Environments.Add(converted);
            }
        }

        /// <summary>
        /// The environments available for exporting.
        /// </summary>
        public ObservableCollection<EnvironmentExportWindowViewModel> Environments { get; set; }

        /// <summary>
        /// Determines if the window will export only a user-selected subset of environments..
        /// </summary>
        public bool ExportSelected
        {
            get => _exportSelected;
            set
            {
                _exportSelected = value;
                OnPropertyChanged(nameof(ExportSelected));
            }
        }

        /// <summary>
        /// Determines if the window will export all environments.
        /// </summary>
        public bool ExportAll
        {
            get => _exportAll;
            set
            {
                _exportAll = value;
                OnPropertyChanged(nameof(ExportAll));
            }
        }

        /// <summary>
        /// Perform the exporting asynchronously
        /// </summary>
        public async Task ExportAsync()
        {
            var environmentList = new List<Environment>();
            if (ExportAll)
            {
                environmentList = _repository.GetEnvironments() as List<Environment>;
            }
            else
            {
                foreach (var environmentViewModel in SelectedEnvironments)
                {
                    var environmentId = environmentViewModel.Environment.Id;
                    var environment = _repository.GetEnvironmentById(environmentId);
                    environmentList.Add(environment);
                }
            }

            IChamiExporter exporter;
            if (Filename.EndsWith("json"))
            {
                exporter = new EnvironmentJsonExporter(environmentList);
            }
            else
            {
                exporter = new EnvironmentExcelExporter(environmentList);
            }

            await exporter.ExportAsync(Filename);
        }

        /// <summary>
        /// The list of environments selected in the listview.
        /// </summary>
        public ObservableCollection<EnvironmentExportWindowViewModel> SelectedEnvironments { get; set; }

        /// <summary>
        /// Adds or removes environments from the <see cref="SelectedEnvironments"/> property when the user changes
        /// its selection.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">Information about the changed selection.</param>
        public void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var removedItems = e.RemovedItems;
            foreach (var removedItem in removedItems)
            {
                SelectedEnvironments.Remove(removedItem as EnvironmentExportWindowViewModel);
            }

            var addedItems = e.AddedItems;
            foreach (var addedItem in addedItems)
            {
                SelectedEnvironments.Add(addedItem as EnvironmentExportWindowViewModel);
            }
        }

        private bool _exportSelected;
        private bool _exportAll;
        private string _filename;

        /// <summary>
        /// The name of the file to export to.
        /// </summary>
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                OnPropertyChanged(nameof(Filename));
                OnPropertyChanged(nameof(ExportButtonEnabled));
            }
        }

        /// <summary>
        /// Determines if the export button is enabled or not.
        /// </summary>
        public bool ExportButtonEnabled => !string.IsNullOrWhiteSpace(Filename);
    }
}