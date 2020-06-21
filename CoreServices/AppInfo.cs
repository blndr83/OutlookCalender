using FileSystemInterfaces;
using Models;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CoreServices
{
    public class AppInfo : IAppInfo
    {
        public string DriveName => _appDrive.DriveName;
        public string DriveFreeSpaceInGigaBytes => $"{_appDrive.FreeSpace / GigaByte: 0.00} GB";

        public string DatabaseSizeInMegaBytes => $"{new FileInfo(_dataSourceProvider.DatabasePath).Length / MegaByte: 0.00} MB";
        private readonly IDataSourceProvider _dataSourceProvider;
        private readonly IAppDrive _appDrive;
        const float GigaByte = 1000000000.0f;
        const float MegaByte = 1000000.0f;

        public AppInfo(IDataSourceProvider dataSourceProvider, IAppDrive appDrive)
        {
            _dataSourceProvider = dataSourceProvider;
            _appDrive = appDrive;
        }

    }
}
