using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Utils;

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
        public ExportWindowViewModel(ExportService exportService)
        {
            ExportAll = true;
            ExportSelected = false;
            Environments = new ObservableCollection<EnvironmentExportWindowViewModel>();
            ExportCommand = new AsyncCommand(async () => await ExportAsync(new CancellationToken()), CanExport);
            CloseCommand = new AsyncCommand<Window>(ExecuteCloseWindow);
            _exportService = exportService;
            var task = _exportService.GetExportableEnvironments(new CancellationToken());
            task.Await();

            Environments = task.Result;
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                ExportCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanExport(object arg)
        {
            return ExportButtonEnabled && Environments.Any(e => e.Environment.IsSelected);
        }

        private readonly ExportService _exportService;

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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        
        public IAsyncCommand ExportCommand { get; }

        /// <summary>
        /// Perform the exporting asynchronously
        /// </summary>
        private async Task ExportAsync(CancellationToken cancellationToken)
        {
            List<EnvironmentViewModel> environmentList;
            if (ExportAll)
            {
                environmentList = await _exportService.GetEnvironmentsFromDataAdapter(cancellationToken);
            }
            else
            {
                var selectedIds = Environments.Where(e => e.Environment.IsSelected).Select(e => e.Environment.Id);
                environmentList = await _exportService.GetEnvironmentsByIdsFromDataAdapter(selectedIds);
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(ExportButtonEnabled));
                ExportCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Determines if the export button is enabled or not.
        /// </summary>
        public bool ExportButtonEnabled => !string.IsNullOrWhiteSpace(Filename);
    }
}