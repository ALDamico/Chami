using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Exporters
{
    public class EnvironmentBatchFileExporter : IPreviewableChamiExporter
    {
        public EnvironmentBatchFileExporter(ScriptExportInfo scriptExportInfo)
        {
            _scriptExportInfo = scriptExportInfo;
        }
        public void Export(string filename)
        {
            ExportAsync(filename).GetAwaiter().GetResult();
        }

        public Task ExportAsync(string filename)
        {
            var sw = GetStream();
            var streamReader = new StreamReader(sw);
            File.WriteAllTextAsync(filename, streamReader.ReadToEnd());
            
            sw.Close();

            return Task.CompletedTask;
        }

        private readonly ScriptExportInfo _scriptExportInfo;

        private void WriteRemarks(StreamWriter sw, string remarks)
        {
            var remarksLength = remarks.Length;
            var charIdx = 0;
            StringBuilder fullStringBuilder = new StringBuilder();
           
            while (charIdx < remarksLength)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("REM ");
                while (charIdx < remarksLength && stringBuilder.Length < _scriptExportInfo.MaxLineLength - 1)
                {
                    stringBuilder.Append(remarks[charIdx]);
                    charIdx++;
                }

                fullStringBuilder.AppendLine(stringBuilder.ToString());
            }


            sw.Write(fullStringBuilder.ToString());
        }

        public string GetPreview(string filename)
        {
            return GetPreviewAsync(filename).GetAwaiter().GetResult();
        }

        public Task<string> GetPreviewAsync(string filename)
        {
            var sw = GetStream();

            sw.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(sw);
            return streamReader.ReadToEndAsync();
        }

        private Stream GetStream()
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            

            var remarks = _scriptExportInfo.Remarks;
            if (!string.IsNullOrWhiteSpace(remarks))
            {
                WriteRemarks(sw, remarks);
            }
            sw.WriteLine();
            sw.WriteLine();
            
            sw.WriteLine($"ECHO Switching environment to {_scriptExportInfo.Environment.Name}");

            foreach (var environmentVariable in _scriptExportInfo.Environment.EnvironmentVariables)
            {
                var variableName = environmentVariable.Name;
                var variableValue = environmentVariable.Value;
                
                sw.WriteLine($"ECHO Setting {variableName} to {variableValue}");
                
                sw.WriteLine($"SETX \"{variableName}\" \"{variableValue}\"");
                sw.WriteLine();
            }
            
            sw.WriteLine("ECHO Execution complete");

            return stream;
        }
    }
}