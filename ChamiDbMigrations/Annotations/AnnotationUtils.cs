using System;
using System.Reflection;
using Chami.Db.Entities;

namespace Chami.Db.Annotations
{
    /// <summary>
    /// Helper classes for annotations.
    /// </summary>
    public static class AnnotationUtils
    {
        /// <summary>
        /// Helper function to get the <see cref="TableNameAttribute"/> for a particular entity.
        /// </summary>
        /// <param name="entityType">The <see cref="Type"/> of the entity we want to inquire.</param>
        /// <returns>The table name.</returns>
        /// <exception cref="NotSupportedException">If the type is not an <see cref="IChamiEntity"/> or if the entity doesn't have a <see cref="TableNameAttribute"/>, a <see cref="NotSupportedException"/> is thrown.</exception>
        public static string GetTableName(Type entityType)
        {
            if (entityType.GetInterface(nameof(IChamiEntity)) != null)
            {
                var tableNameAttribute = entityType.GetCustomAttribute<TableNameAttribute>();
                if (tableNameAttribute != null)
                {
                    return tableNameAttribute.TableName;
                }
                else
                {
                    throw new NotSupportedException("Attribute not found!");                    
                }
            }

            throw new NotSupportedException($"The type {entityType.FullName} is not an IChamiEntity");
        }
    }
}