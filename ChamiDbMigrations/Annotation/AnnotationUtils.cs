using System;
using Chami.Db.Entities;

namespace Chami.Db.Annotation
{
    public static class AnnotationUtils
    {
        public static string GetTableName(Type entityType)
        {
            if (entityType.GetInterface(nameof(IChamiEntity)) != null)
            {
                var tableNameAttributes = entityType.GetCustomAttributes(typeof(TableNameAttribute), true);
                if (tableNameAttributes is {Length: > 0})
                {
                    if (tableNameAttributes[0] is TableNameAttribute tableNameAttribute)
                    {
                        return tableNameAttribute.TableName;
                    }
                }
            }

            throw new NotSupportedException("Attribute not found!");
        }
    }
}