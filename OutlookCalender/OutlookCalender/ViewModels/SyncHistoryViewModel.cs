using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlookCalender.ViewModels
{
    public class SyncHistoryViewModel : ViewModelBase
    {
        public Command ApplyFilterCommand { get; }
        public Command UndoFilterCommand { get; }
        public Command DeleteHistoryCommand { get; }
        public Command<Guid> DeleteLogCommand { get; }
        public ReadOnlyObservableCollection<SyncLog> SyncLogs { get; }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value, OnFilterDateChanged); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value, OnFilterDateChanged); } }
        public bool ApplyFilterEnabled { get { return _applyFilterEnabled; } private set { SetBackingField(ref _applyFilterEnabled, value); } }
        public bool UndoFilterEnabled { get { return _undoFilterEnabled; } private set { SetBackingField(ref _undoFilterEnabled, value); } }
        public bool DeleteHistoryEnabled { get { return _deleteHistoryEnabled; } private set { SetBackingField(ref _deleteHistoryEnabled, value); } }
        public string DeleteHistoryButtonText { get { return _deleteHistoryButtonText; } private set { SetBackingField(ref _deleteHistoryButtonText, value); } }

        private readonly ObservableCollection<SyncLog> _syncLogsInternal;
        private readonly IRepository _repository;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _applyFilterEnabled;
        private bool _undoFilterEnabled;
        private bool _deleteHistoryEnabled;
        private string _deleteHistoryButtonText;
        private readonly Func<string, Task<bool>> _displayAlert;
        private const string DeleteHistoryButtonTextStandard = "Delete History";
        private const string DeleteHistoryButtonTextFiltered = "Delete filtered History";

        public SyncHistoryViewModel(IRepository repository, Func<string, Task<bool>> displayAlert)
        {
            _repository = repository;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            ApplyFilterCommand = new Command(ApplyFilter);
            UndoFilterCommand = new Command(UndoFilter);
            ApplyFilterEnabled = true;
            DeleteLogCommand = new Command<Guid>(DeleteLog);
            _syncLogsInternal = new ObservableCollection<SyncLog>();
            SyncLogs = new ReadOnlyObservableCollection<SyncLog>(_syncLogsInternal);
            DeleteHistoryCommand = new Command(DeleteHistory);
            _displayAlert = displayAlert;
            DeleteHistoryButtonText = DeleteHistoryButtonTextStandard;
        }

        private void OnFilterDateChanged(DateTime oldValue)
        {
            ApplyFilterEnabled = _startDate <= _endDate;
        }

        public void InitSyncLogCollection()
        {
            if(!_undoFilterEnabled)
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
        
        private void DeleteHistory()
        {
            const string filtered = "filtered";
            const string complete = "complete";
            var message = $"Do you realy want to delete the {(UndoFilterEnabled ? filtered : complete)} History";
            Device.BeginInvokeOnMainThread(async () =>
            {
                var delete = await _displayAlert(message);
                if(delete)
                {

                }
            });
            
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
                    syncLogs.OrderByDescending(_ => _.SyncDate).ToList().ForEach(s => _syncLogsInternal.Add(s));
                }
                DeleteHistoryEnabled = _syncLogsInternal.Any();
                UndoFilterEnabled = undoFilteEnabled;
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
