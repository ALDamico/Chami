using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Exporters
{
    public class ScriptExportInfo
    {
        public string Remarks { get; set; }
        public int MaxLineLength { get; set; }
        public EnvironmentViewModel Environment { get; set; }
    }
}