using System;

namespace ChamiUI.BusinessLayer.Exceptions.Exporter;

public class ExceptionExportModel
{
    public string ExceptionName { get; set; }
    public string ExceptionSource { get; set; }
    public string StackTrace { get; set; }
    public string ExceptionMessage { get; set; }
    public string Log { get; set; }
    public DateTime GenerationDate { get; set; }
    public string OperatingSystem { get; set; }
}