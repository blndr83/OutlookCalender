using System;

namespace FileSystemInterfaces
{
    public interface IAppDrive
    {
        string DriveName { get; }
        long FreeSpace { get; }
    }
}
