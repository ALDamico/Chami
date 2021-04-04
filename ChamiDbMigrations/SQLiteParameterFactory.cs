using System;
using System.Data.SQLite;

namespace ChamiDbMigrations
{
    internal static class SQLiteParameterFactory
    {
        internal static SQLiteParameter Convert(object value)
        {
            var param = new SQLiteParameter();
            param.Value = value;
            return param;
        }
    }
}