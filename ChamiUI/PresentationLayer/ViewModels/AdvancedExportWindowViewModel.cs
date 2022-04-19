using System;
using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Exporters;
using NetOffice.ExcelApi;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExportWindowViewModel : ViewModelBase
    {
        public AdvancedExportWindowViewModel()
        {
            LineMaxLength = 80;
            Environments = new ObservableCollection<EnvironmentViewModel>();
            AvailableExporters = new ObservableCollection<AdvancedExporterViewModel>();
            
            //AvailableExporters.Add(new AdvancedExporterViewModel(){new EnvironmentBatchFileExporter()});
        }
        private string _scriptPath;
        private EnvironmentViewModel _selectedEnvironment;
        private bool _includeRemarks;
        private string _remarks;
        private int _lineMaxLength;
        private string _preview;

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
        
        // TODO Implement IEnvironmentExportStrategy

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
        
        public ObservableCollection<AdvancedExporterViewModel> AvailableExporters { get; }

        public void GeneratePreview()
        {
            var exportInfo = new ScriptExportInfo()
            {
                Environment = SelectedEnvironment,
                MaxLineLength = LineMaxLength,
                Remarks = Remarks
            };

            var exporter = new EnvironmentBatchFileExporter(exportInfo);
            Preview = exporter.GetPreview(Guid.NewGuid().ToString());
        }
    }
}