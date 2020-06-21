using System;
using FileSystemInterfaces;

namespace OutlookCalender.iOS
{
    public class AppDrive : IAppDrive
    {
        public string DriveName => throw new NotImplementedException();

        public long FreeSpace => throw new NotImplementedException();
    }
}