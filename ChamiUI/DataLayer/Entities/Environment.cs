using System;
using System.Collections.Generic;

namespace ChamiUI.DataLayer.Entities
{
    public class Environment
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
    }
}