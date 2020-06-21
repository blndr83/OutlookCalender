namespace CoreServices
{
    public interface IAppInfo
    {
        string DriveName { get; }
        string DriveFreeSpaceInGigaBytes { get; }
        string DatabaseSizeInMegaBytes { get; }
    }
}
