using FileSystemInterfaces;
using System;

namespace OutlookCalender.UWP
{
    public class AppDrive : IAppDrive
    {
        public string DriveName => throw new NotImplementedException();

        public long FreeSpace => throw new NotImplementedException();
    }
}
