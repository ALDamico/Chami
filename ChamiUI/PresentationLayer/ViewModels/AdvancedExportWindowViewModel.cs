using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExportWindowViewModel : ViewModelBase
    {
        public AdvancedExportWindowViewModel()
        {
            var settings = SettingsUtils.GetAppSettings().AdvancedExporterSettings;
            LineMaxLength = 80;
            if (settings.SessionMaxLineLength.HasValue)
            {
                LineMaxLength = settings.SessionMaxLineLength.Value;
            }

            Environments = new ObservableCollection<EnvironmentViewModel>();
            AvailableExporters = new ObservableCollection<AdvancedExporterViewModel>
            {
                new AdvancedExporterViewModel()
                {
                    AdvancedExporterFactory = (scriptEportInfo) => new EnvironmentBatchFileExporter(scriptEportInfo),
                    DisplayName = ChamiUIStrings.EnvironmentBatchFileExporterDisplayName,
                    Description = ChamiUIStrings.EnvironmentBatchFileExporterDescription
                },
                new AdvancedExporterViewModel()
                {
                    AdvancedExporterFactory = scriptExportInfo => new EnvironmentPowerShellScriptExporter(scriptExportInfo),
                    DisplayName = ChamiUIStrings.EnvironmentPowerShellScriptExporterDisplayName,
                    Description = ChamiUIStrings.EnvironmentPowerShellScriptExporterDescription
                }
            };

            SelectedExporter = AvailableExporters.First();
        }

        private string _scriptPath;
        private EnvironmentViewModel _selectedEnvironment;
        private bool _includeRemarks;
        private string _remarks;
        private int _lineMaxLength;
        private string _preview;
        private AdvancedExporterViewModel _selectedExporter;
        private Brush _previewBackgroundColor;
        private Brush _previewForegroundColor;

        public Brush PreviewForegroundColor
        {
            get => _previewForegroundColor;
            set
            {
                _previewForegroundColor = value;
                OnPropertyChanged(nameof(PreviewForegroundColor));
            }
        }

        public Brush PreviewBackgroundColor
        {
            get => _previewBackgroundColor;
            set
            {
                _previewBackgroundColor = value;
                OnPropertyChanged(nameof(PreviewBackgroundColor));
            }
        }

        public string Preview
        {
            get => _preview;
            set
            {
                _preview = value;
                OnPropertyChanged(nameof(Preview));
            }
        }

        public int LineMaxLength
        {
            get => _lineMaxLength;
            set
            {
                _lineMaxLength = value;
                OnPropertyChanged(nameof(LineMaxLength));
            }
        }

        public string Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }

        public bool IncludeRemarks
        {
            get => _includeRemarks;
            set
            {
                _includeRemarks = value;
                OnPropertyChanged(nameof(IncludeRemarks));
            }
        }

        public EnvironmentViewModel SelectedEnvironment
        {
            get => _selectedEnvironment;
            set
            {
                _selectedEnvironment = value;
                OnPropertyChanged(nameof(SelectedEnvironment));
            }
        }

        public string ScriptPath
        {
            get => _scriptPath;
            set
            {
                _scriptPath = value;
                OnPropertyChanged(nameof(ScriptPath));
            }
        }
        
        public ObservableCollection<EnvironmentViewModel> Environments { get; }

        public ObservableCollection<AdvancedExporterViewModel> AvailableExporters { get; }

        public AdvancedExporterViewModel SelectedExporter
        {
            get => _selectedExporter;
            set
            {
                _selectedExporter = value;
                OnPropertyChanged(nameof(SelectedExporter));
            }
        }

        public void GeneratePreview()
        {
            var exportInfo = new ScriptExportInfo()
            {
                Environment = SelectedEnvironment,
                MaxLineLength = LineMaxLength,
                Remarks = Remarks
            };

            var exporter = SelectedExporter.AdvancedExporterFactory.Invoke(exportInfo);
            Preview = exporter.GetPreview();
        }
        
        public void ClearMarkedVariables(EnvironmentViewModel oldEnvironment, EnvironmentViewModel newEnvironment)
        {
            if (oldEnvironment != null)
            {
                foreach (var element in oldEnvironment.EnvironmentVariables)
                {
                    element.MarkedForExporting = false;
                }
            }

            if (newEnvironment == null)
            {
                return;
            }

            foreach (var element in newEnvironment.EnvironmentVariables)
            {
                element.MarkedForExporting = true;
            }
        }

        public void SetAllVariables(EnvironmentViewModel environment, bool targetValue)
        {
            if (environment == null)
            {
                return;
            }

            var environmentVariables = environment.EnvironmentVariables;

            foreach (var environmentVariable in environmentVariables)
            {
                environmentVariable.MarkedForExporting = targetValue;
            }
        }
    }
}