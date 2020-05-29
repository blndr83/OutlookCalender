using Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlookCalender.ViewModels
{
    public class SyncHistoryViewModel : ViewModelBase
    {
        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand UndoFilterCommand { get; }
        public AsyncRelayCommand DeleteHistoryCommand { get; }
        public Command<Guid> DeleteLogCommand { get; }
        public ReadOnlyObservableCollection<SyncLog> SyncLogs { get; }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value, OnFilterDateChanged); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value, OnFilterDateChanged); } }
        public string DeleteHistoryButtonText { get { return _deleteHistoryButtonText; } private set { SetBackingField(ref _deleteHistoryButtonText, value); } }

        private readonly ObservableRangeCollection<SyncLog> _syncLogsInternal;
        private readonly IRepository _repository;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _deleteHistoryButtonText;
        private readonly Func<string, Task<bool>> _displayAlert;
        private const string DeleteHistoryButtonTextStandard = "Delete History";
        private const string DeleteHistoryButtonTextFiltered = "Delete filtered History";

        public SyncHistoryViewModel(IRepository repository, Func<string, Task<bool>> displayAlert)
        {
            _repository = repository;
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            UndoFilterCommand = new RelayCommand(UndoFilter) { IsEnabled = false};
            DeleteLogCommand = new Command<Guid>(DeleteLog);
            _syncLogsInternal = new ObservableRangeCollection<SyncLog>();
            SyncLogs = new ReadOnlyObservableCollection<SyncLog>(_syncLogsInternal);
            DeleteHistoryCommand = new AsyncRelayCommand(DeleteHistory);
            _displayAlert = displayAlert;
            DeleteHistoryButtonText = DeleteHistoryButtonTextStandard;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        private void OnFilterDateChanged(DateTime oldValue)
        {
            ApplyFilterCommand.IsEnabled = _startDate <= _endDate;
        }

        public void InitSyncLogCollection()
        {
            if(!UndoFilterCommand.IsEnabled)
            {
                _syncLogsInternal.Clear();
                Task.Run(async () =>
                {
                    var syncLogs = await _repository.GetAll<SyncLog>();
                    UpdateSyncLogCollection(syncLogs, false);
                });
   
            }
        }

        private void ApplyFilter()
        {
            _syncLogsInternal.Clear();
            Task.Run(async () =>
            {
                var syncLogs = await _repository.FindAll<SyncLog>(_ => _.StartDate <= _startDate && _.EndDate >= _endDate);
                 UpdateSyncLogCollection(syncLogs, true);
            });
   
        }
        
        private async Task DeleteHistory()
        {
            const string filtered = "filtered";
            const string complete = "complete";
            var message = $"Do you realy want to delete the {(UndoFilterCommand.IsEnabled ? filtered : complete)} History";
            var delete = await _displayAlert(message);
            if(delete)
            {
                var itemsToDelete = UndoFilterCommand.IsEnabled ? await _repository.FindAll<SyncLog>(_ => _.StartDate <= _startDate && _.EndDate >= _endDate) : await _repository.GetAll<SyncLog>();
                if(itemsToDelete.Any())
                {
                    _repository.DeleteRange(itemsToDelete);
                    _syncLogsInternal.Clear();
                }
            }
            
        }

        private void DeleteLog(Guid id)
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                var syncLog = _syncLogsInternal.SingleOrDefault(_ => _.Id == id);
                var delete = await _displayAlert($"Do you realy want to delete the Log Entry with sync the range {syncLog.StartDate.ToShortDateString()} - {syncLog.EndDate.ToShortDateString()} that was done on {syncLog.SyncDate.ToShortDateString()}");
                if(delete)
                {
                    _syncLogsInternal.Remove(syncLog);
                    _repository.Delete(syncLog);
                }
            });
        }

        private void UpdateSyncLogCollection(IList<SyncLog> syncLogs, bool undoFilteEnabled)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (syncLogs.Any())
                {
                    _syncLogsInternal.AddRange(syncLogs.OrderByDescending(_ => _.SyncDate).ToList());
                }
                DeleteHistoryCommand.IsEnabled = _syncLogsInternal.Any();
                UndoFilterCommand.IsEnabled = undoFilteEnabled;
                 DeleteHistoryButtonText = undoFilteEnabled ? DeleteHistoryButtonTextFiltered : DeleteHistoryButtonTextStandard;
            });
        }

        private void UndoFilter ()
        {
            _syncLogsInternal.Clear();
            Task.Run(async () =>
            {
                var syncLogs = await _repository.GetAll<SyncLog>();
                UpdateSyncLogCollection(syncLogs, false);
            });
   
        }
    }
}
