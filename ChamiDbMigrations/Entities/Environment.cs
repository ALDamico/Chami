using System;
using System.Collections.Generic;
using Chami.Db.Annotation;

namespace Chami.Db.Entities
{
    [TableName("Environments")]
    public class Environment : IChamiEntity
    {
        public Environment()
        {
            AddedOn = DateTime.Now;
            EnvironmentVariables = new List<EnvironmentVariable>();
        }
        public int EnvironmentId { get; set; }
        public string Name { get; set; }
        public DateTime AddedOn { get; set; }
        public ICollection<EnvironmentVariable> EnvironmentVariables { get; }
        public EnvironmentType EnvironmentType { get; set; }
        //public bool IsBackup { get; set; }
    }
}