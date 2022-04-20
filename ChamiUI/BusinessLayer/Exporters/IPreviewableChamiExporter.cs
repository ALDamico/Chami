using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Exporters
{
    public interface IPreviewableChamiExporter : IChamiExporter
    {
        string GetPreview();
        Task<string> GetPreviewAsync();
    }
}