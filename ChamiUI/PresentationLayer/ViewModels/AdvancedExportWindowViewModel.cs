using System.Collections.ObjectModel;
using NetOffice.ExcelApi;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExportWindowViewModel : ViewModelBase
    {
        public AdvancedExportWindowViewModel()
        {
            Environments = new ObservableCollection<EnvironmentViewModel>();
        }
        private string _scriptPath;
        private EnvironmentViewModel _selectedEnvironment;
        private bool _includeRemarks;
        private string _remarks;

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

        public void ClearMarkedVariables(EnvironmentViewModel viewModel)
        {
            if (viewModel != null)
            {
                foreach (var element in viewModel.EnvironmentVariables)
                {
                    element.MarkedForExporting = false;
                }
            }
        }
    }
}