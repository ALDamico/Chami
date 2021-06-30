using System;
using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("EnvironmentVariables")]
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