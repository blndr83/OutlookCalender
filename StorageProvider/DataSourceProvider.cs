using System.IO;

namespace StorageProvider
{
    public class DataSourceProvider
    {
        private const string DB = "Calendar.db";
        public static string DbPath { get; private set; }

        public static void SetDbPath(string path)
        {
            DbPath = Path.Combine(path, DataSourceProvider.DB);
        }
    }
}
