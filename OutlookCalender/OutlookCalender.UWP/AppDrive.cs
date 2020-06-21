using FileSystemInterfaces;
using System;
using System.IO;
using Windows.Storage;

namespace OutlookCalender.UWP
{
    public class AppDrive : IAppDrive
    {
        public string DriveName { get; }

        public long FreeSpace => Convert.ToInt64(StorageFolder.GetFolderFromPathAsync(AppDomain.CurrentDomain.BaseDirectory).AsTask().Result.Properties.RetrievePropertiesAsync(new[] { "System.FreeSpace"}).AsTask().Result["System.FreeSpace"]);
        
        public AppDrive()
        {
            DriveName = Directory.GetDirectoryRoot(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
