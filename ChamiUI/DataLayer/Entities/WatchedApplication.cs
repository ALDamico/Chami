using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamiUI.DataLayer.Entities
{
    public class WatchedApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWatchEnabled { get; set; }
    }
}
