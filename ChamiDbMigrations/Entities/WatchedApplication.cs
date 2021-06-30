using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    /// <summary>
    /// Represents a row in the WatchedApplications table.
    /// These are the names of the applications that Chami will notify the user, if they so choose, when they change the
    /// current environment and the application detects they are running.
    /// </summary>
    [TableName("WatchedApplications")]
    public class WatchedApplication : IChamiEntity
    {
        /// <summary>
        /// The primary key of the WatchedApplications table.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name (as shown in Windows Task Manager) of the application to detect.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Specifies whether the user wishes to be notified about this application.
        /// </summary>
        public bool IsWatchEnabled { get; set; }
    }
}
