using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Exporters;

public interface ILocalizationExporter
{
    void Export();
    Task ExportAsync();
}