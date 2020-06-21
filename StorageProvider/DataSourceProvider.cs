using Models;
using System.IO;

namespace StorageProvider
{
    public class DataSourceProvider : IDataSourceProvider
    {
        private const string DB = "Calendar.db";
        public static string DbPath { get; private set; }

        public string DatabasePath => DbPath;

        public static void SetDbPath(string path)
        {
            DbPath = Path.Combine(path, DB);
        }
    }
}
