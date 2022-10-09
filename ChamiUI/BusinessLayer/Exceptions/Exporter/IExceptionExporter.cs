using System;

namespace ChamiUI.BusinessLayer.Exceptions.Exporter;

public interface IExceptionExporter
{
    string FileName { get; }
    void Export(Exception exception, string logPath);
}