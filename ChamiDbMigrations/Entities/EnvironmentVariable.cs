using System;
using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    /// <summary>
    /// Represents a single <see cref="EnvironmentVariable"/> in an <see cref="Environment"/>
    /// </summary>
    [TableName("EnvironmentVariables")]
    public class EnvironmentVariable : IChamiEntity
    {
        public bool MarkedForDeletion { get; set; }

        /// <summary>
        /// Constructs a new <see cref="EnvironmentVariable"/> object with default values.
        /// </summary>
        public EnvironmentVariable()
        {
            AddedOn = DateTime.Now;
        }

        /// <summary>
        /// The primary key in the EnvironmentVariables table.
        /// </summary>
        public int EnvironmentVariableId { get; set; }

        /// <summary>
        /// The name of the <see cref="EnvironmentVariable"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the <see cref="EnvironmentVariable"/>
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The foreign key to the <see cref="Environment"/>s table.
        /// </summary>
        public int EnvironmentId { get; set; }

        /// <summary>
        /// The date and time when this <see cref="EnvironmentVariable"/> was created.
        /// </summary>
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// The <see cref="Environment"/> this <see cref="EnvironmentVariable"/> belongs to.
        /// </summary>
        public Environment Environment { get; set; }
        
        /// <summary>
        /// True if the <see cref="EnvironmentVariable"/> entity is a path to a folder, otherwise false.
        /// </summary>
        public bool? IsFolder { get; set; }
    }
}