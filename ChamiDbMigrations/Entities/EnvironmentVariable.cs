using System;

namespace Chami.Db.Entities
{
    public class EnvironmentVariable : IChamiEntity
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