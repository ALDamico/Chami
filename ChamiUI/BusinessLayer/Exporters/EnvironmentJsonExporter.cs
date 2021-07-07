using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Converters;
using Newtonsoft.Json;

namespace ChamiUI.BusinessLayer.Exporters
{
    public class EnvironmentJsonExporter : IChamiExporter
    {
        public EnvironmentJsonExporter(ICollection<Environment> environments) : this()
        {
            _environments = environments.ToList();
        }

        public EnvironmentJsonExporter()
        {
            _environments = new List<Environment>();
        }

        private readonly List<Environment> _environments;


        public void Export(string filename)
        {
            var writer = File.CreateText(filename);
            writer.Write(JsonConvert.SerializeObject(_environments, Formatting.Indented,
                new JsonConverter[] {new EnvironmentViewModelJsonConverter()}));
            writer.Close();
        }

        public async Task ExportAsync(string filename)
        {
            var writer = File.CreateText(filename);
            var text = JsonConvert.SerializeObject(_environments, Formatting.Indented,
                new JsonConverter[] {new EnvironmentViewModelJsonConverter()});
            await writer.WriteAsync(text);
            writer.Close();
        }
    }
}