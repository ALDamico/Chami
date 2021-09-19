using System;

namespace Chami.Db.Entities
{
    public class EnvironmentVariableBlacklist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InitialValue { get; set; }
        public bool IsWindowsDefault { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime AddedOn { get; set; }
        
    }
}