using System;
using System.Collections.Generic;
using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    /// <summary>
    /// Represents a set of <see cref="EnvironmentVariables"/> that belong together.
    /// </summary>
    [TableName("Environments")]
    public class Environment : IChamiEntity
    {
        /// <summary>
        /// Constructs a new <see cref="Environment"/> object with default values.
        /// These are:
        /// <see cref="AddedOn"/>: Current date and time.
        /// <see cref="EnvironmentVariables"/> An empty <see cref="List{T}"/>
        /// </summary>
        public Environment()
        {
            AddedOn = DateTime.Now;
            EnvironmentVariables = new List<EnvironmentVariable>();
        }
        /// <summary>
        /// The unique identifier of the <see cref="Environment"/> entity.
        /// </summary>
        public int EnvironmentId { get; set; }
        /// <summary>
        /// An arbitary name that identifies this <see cref="Environment"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The date and time when this <see cref="Environment"/> was created.
        /// </summary>
        public DateTime AddedOn { get; set; }
        /// <summary>
        /// A list of <see cref="EnvironmentVariables"/> that belong to this <see cref="Environment"/>
        /// </summary>
        public ICollection<EnvironmentVariable> EnvironmentVariables { get; }
        /// <summary>
        /// Determines what type of environment this is.
        /// </summary>
        /// <seealso cref="EnvironmentType"/>
        public EnvironmentType EnvironmentType { get; set; }
        
        public List<EnvironmentTagAssociation> TagAssociations { get; set; }
    }
}