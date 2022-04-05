using System;
using System.Text.RegularExpressions;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public class TimeSpanConverter
    {
        public TimeSpan Convert(Setting entity)
        {
            if (Regex.IsMatch(entity.Value, "[0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2}"))
            {
                return TimeSpan.Parse(entity.Value);
            }
            var wasConverted = double.TryParse(entity.Value, out var milliseconds);
            if (!wasConverted)
            {
                return TimeSpan.MaxValue;
            }
            return TimeSpan.FromMilliseconds(milliseconds);
        }
    }
}