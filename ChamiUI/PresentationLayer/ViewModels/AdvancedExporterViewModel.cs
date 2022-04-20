using System;
using ChamiUI.BusinessLayer.Exporters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterViewModel : GenericLabelViewModel
    {
        public Func<ScriptExportInfo, IPreviewableChamiExporter> AdvancedExporterFactory { get; set; }
    }
}