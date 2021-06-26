namespace Chami.Db.Entities
{
    public class WatchedApplication : IChamiEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWatchEnabled { get; set; }
    }
}
