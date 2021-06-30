using System;

namespace Chami.Db.Annotations
{
    /// <summary>
    /// Used to mark entities to their corresponding table names. Used for migrations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the table this entity maps to.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Constructs a new <see cref="TableNameAttribute"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table (mandatory).</param>
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}