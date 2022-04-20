using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Exporters;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Utils
{
    public static class ExporterUtils
    {
        public static ObservableCollection<AdvancedExporterViewModel> GetAvailableExporters()
        {
            return new ObservableCollection<AdvancedExporterViewModel>
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
        }
    }
}