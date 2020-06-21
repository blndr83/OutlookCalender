using Android.OS;
using FileSystemInterfaces;

namespace OutlookCalender.Droid
{
    public class AppDrive : IAppDrive
    {
        public string DriveName => Environment.ExternalStorageDirectory.AbsolutePath;

        public long FreeSpace => Environment.ExternalStorageDirectory.FreeSpace;
    }
}