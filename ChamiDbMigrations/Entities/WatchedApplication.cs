namespace ChamiDbMigrations.Entities
{
    public class WatchedApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWatchEnabled { get; set; }
    }
}
