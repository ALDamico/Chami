using System;
using Chami.Db.Entities;
using Serilog.Events;

namespace ChamiUI.BusinessLayer.Converters;

public class LogEventLevelConverter 
{
    public LogEventLevel Convert(Setting setting)
    {
        return Enum.Parse<LogEventLevel>(setting.Value);
    }
}