using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ChamiUI.BusinessLayer.Exceptions.Exporter;

public class ExceptionExporter : IExceptionExporter
{
    public ExceptionExporter(string fileName)
    {
        FileName = fileName;
    }
    public string FileName { get; set; }
    public void Export(Exception exception, string logPath)
    {
        var model = ExceptionExportModelFactory.GetExceptionExportModel(exception, logPath);

        var json = JsonConvert.SerializeObject(model, Formatting.Indented);
        using var f = new StreamWriter(FileName);
        f.Write(json);
    }
}