using System;

namespace ChamiUI.DataLayer.Entities
{
    public class EnvironmentVariable
    {
        public EnvironmentVariable()
        {
            AddedOn = DateTime.Now;
        }

        public int EnvironmentVariableId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int EnvironmentId { get; set; }
        public DateTime AddedOn { get; set; }
        public Environment Environment { get; set; }
    }
}