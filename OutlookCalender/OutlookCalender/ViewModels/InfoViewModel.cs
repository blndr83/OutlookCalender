using CoreServices;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OutlookCalender.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        public string FreeInternalDiskSpace { get { return _freeInternalDiskSpace; } private set { SetBackingField(ref _freeInternalDiskSpace, value); } }
        public string DatabaseSize { get { return _databaseSize; } private set { SetBackingField(ref _databaseSize, value); } }
        public int AmountOfCalendarEntries { get { return _amountOfCalendarEntries; } private set { SetBackingField(ref _amountOfCalendarEntries, value); } }
        public int AmountOfSyncLogs { get { return _amountOfSyncLogs; } private set { SetBackingField(ref _amountOfSyncLogs, value); } }
        public string InternalDiskName { get; }

        private string _freeInternalDiskSpace;
        private string _databaseSize;
        private int _amountOfCalendarEntries;
        private int _amountOfSyncLogs;
        private readonly IRepository _repository;
        private readonly IAppInfo _appInfo;

        public InfoViewModel(IRepository repository, IAppInfo appDriveInfo)
        {
            _repository = repository;
            _appInfo = appDriveInfo;
            InternalDiskName = _appInfo.DriveName;
        }

        public void UpdateProperties()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                AmountOfCalendarEntries = await _repository.Count<EventModel>();
                AmountOfSyncLogs = await _repository.Count<SyncLog>();
                FreeInternalDiskSpace = _appInfo.DriveFreeSpaceInGigaBytes;
                DatabaseSize = _appInfo.DatabaseSizeInMegaBytes;
            });
        }
    }
}
