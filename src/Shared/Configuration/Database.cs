using System;
using System.IO;
using System.Linq;

namespace Shared.Configuration
{
    public static class Database
    {
        public static string DatabaseConnectionstring(string databaseName)
        {
            var storagePath = FindStoragePath();
            Directory.CreateDirectory(storagePath);
            var databaseLocation = Path.Combine(storagePath, databaseName);
            return $"Filename={databaseLocation}; Connection=shared";
        }

        // public static void Setup(string databaseName, Action<LiteDatabase> configureDatabase = null)
        // {
        //     using var db = new LiteDatabase(DatabaseConnectionstring(databaseName));
        //     
        //     configureDatabase?.Invoke(db);
        // }

        static string FindStoragePath()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            while (true)
            {
                // Finding a solution file takes precedence
                if (Directory.EnumerateFiles(directory).Any(file => file.EndsWith(".sln")))
                {
                    return Path.Combine(directory, DefaultDatabaseDirectory);
                }

                // When no solution file was found try to find a database directory
                var databaseDirectory = Path.Combine(directory, DefaultDatabaseDirectory);
                if (Directory.Exists(databaseDirectory))
                {
                    return databaseDirectory;
                }

                var parent = Directory.GetParent(directory);

                if (parent == null)
                {
                    // throw for now. if we discover there is an edge then we can fix it in a patch.
                    throw new Exception(
                        $"Unable to determine the storage directory path for the database due to the absence of a solution file. Please create a '{DefaultDatabaseDirectory}' directory in one of this project’s parent directories.");
                }

                directory = parent.FullName;
            }
        }

        const string DefaultDatabaseDirectory = ".database";
    }
}