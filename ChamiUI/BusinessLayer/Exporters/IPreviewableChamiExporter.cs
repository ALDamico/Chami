using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Exporters
{
    public interface IPreviewableChamiExporter : IChamiExporter
    {
        string GetPreview(string filename);
        Task<string> GetPreviewAsync(string filename);
    }
}