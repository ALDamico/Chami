using System;
using System.IO;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Utils;
using Microsoft.Extensions.DependencyInjection;
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
        var settingsDataAdapter = AppUtils.GetAppServiceProvider().GetService<SettingsDataAdapter>();
        var model = ExceptionExportModelFactory.GetExceptionExportModel(exception, logPath, settingsDataAdapter);
        

        var json = JsonConvert.SerializeObject(model, Formatting.Indented);
        using var f = new StreamWriter(FileName);
        f.Write(json);
    }
}