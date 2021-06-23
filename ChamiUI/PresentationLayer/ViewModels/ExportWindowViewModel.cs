using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using ChamiDbMigrations.Entities;
using ChamiDbMigrations.Repositories;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Exporters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ExportWindowViewModel : ViewModelBase
    {
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

        public ExportWindowViewModel(ICollection<EnvironmentViewModel> environments) : this()
        {
            var converter = new EnvironmentExportConverter();
            foreach (var environment in environments)
            {
                var converted = converter.From(environment);
                Environments.Add(converted);
            }
        }

        public ObservableCollection<EnvironmentExportWindowViewModel> Environments { get; set; }

        public bool ExportSelected
        {
            get => _exportSelected;
            set
            {
                _exportSelected = value;
                OnPropertyChanged(nameof(ExportSelected));
            }
        }

        public bool ExportAll
        {
            get => _exportAll;
            set
            {
                _exportAll = value;
                OnPropertyChanged(nameof(ExportAll));
            }
        }

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
            

            var exporter = new EnvironmentExcelExporter(environmentList);
            await exporter.ExportAsync(Filename);
        }

        public ObservableCollection<EnvironmentExportWindowViewModel> SelectedEnvironments { get; set; }

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

        public bool ExportButtonEnabled => !string.IsNullOrWhiteSpace(Filename);
    }
}