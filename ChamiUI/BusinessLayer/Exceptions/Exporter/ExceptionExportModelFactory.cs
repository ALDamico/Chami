using System;
using System.IO;
using System.Net;

namespace ChamiUI.BusinessLayer.Exceptions.Exporter;

public static class ExceptionExportModelFactory
{
    public static ExceptionExportModel GetExceptionExportModel(Exception exception, string logPath)
    {
        ExceptionExportModel exceptionExportModel = new()
        {
            ExceptionMessage = exception.Message,
            ExceptionName = exception.GetType().FullName,
            ExceptionSource = exception.Source,
            StackTrace = exception.StackTrace,
            GenerationDate = DateTime.Now,
            OperatingSystem = Environment.OSVersion.ToString()
        };

        using var logFile = File.Open(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var bytes = new byte[logFile.Length]; 
        logFile.Read(bytes, 0, (int)logFile.Length);
        
        exceptionExportModel.Log = System.Text.Encoding.UTF8.GetString(bytes);
        
        return exceptionExportModel;
    }
}