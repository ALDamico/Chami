using System.IO;
using System.Text;
using System.Threading.Tasks;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Exporters;

public class EnvironmentPropertiesFileExporter : IChamiExporter
{
    public EnvironmentPropertiesFileExporter(Environment environment)
    {
        _environment = environment;
    }

    private readonly Environment _environment;

    public void Export(string filename)
    {
        ExportAsync(filename).GetAwaiter().GetResult();
    }

    public async Task ExportAsync(string filename)
    {
        var file = File.CreateText(filename);
        var stringBuilder = new StringBuilder();
        foreach (var variable in _environment.EnvironmentVariables)
        {
            stringBuilder.Clear();
            stringBuilder.Append(variable.Name).Append('=').Append(variable.Value).AppendLine();
            await file.WriteLineAsync(stringBuilder.ToString());
        }

        await file.FlushAsync();
    }
}