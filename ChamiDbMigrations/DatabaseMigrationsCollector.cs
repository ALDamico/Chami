using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ChamiDbMigrations
{
    public class DatabaseMigrationsCollector
    {
        public DatabaseMigrationsCollector(string migrationPath)
        {
            _migrationPath = migrationPath;
        }

        public List<DatabaseMigration> Collect()
        {
            var paths = Directory.GetFiles(_migrationPath, "*.sql");
            var output = new List<DatabaseMigration>();
            int order = 0;
            foreach (var path in paths)
            {
                var fileName = Path.GetFileName(path);
                var dbMigration = new DatabaseMigration();
                dbMigration.Filename = fileName;
                var parts = fileName.Split("_", 2);
                string ordinalString = null;
                if (parts.Length == 2)
                {
                    ordinalString = parts[0];
                }
                if (ordinalString != null)
                {
                    var canConvert = int.TryParse(ordinalString, out int determinedOrder);
                    if (canConvert)
                    {
                        dbMigration.Order = determinedOrder;
                    }
                }
                else
                {
                    dbMigration.Order = order;
                }

                dbMigration.FullPath = path;

                output.Add(dbMigration);
            }

            output.Sort((m1, m2) => m1.Order.CompareTo(m2.Order));
            return output;
        }

        private string _migrationPath;
    }
}