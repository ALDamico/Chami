using System;
using System.Collections.Generic;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Exceptions.Exporter;

public class ExceptionExportModel
{
    public string ExceptionName { get; set; }
    public string ExceptionSource { get; set; }
    public List<string> StackTrace { get; set; }
    public string ExceptionMessage { get; set; }
    public List<string> Log { get; set; }
    public DateTime GenerationDate { get; set; }
    public string OperatingSystem { get; set; }
    public List<Setting> Settings { get; set; }
}