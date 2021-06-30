using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("WatchedApplications")]
    public class WatchedApplication : IChamiEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWatchEnabled { get; set; }
    }
}
