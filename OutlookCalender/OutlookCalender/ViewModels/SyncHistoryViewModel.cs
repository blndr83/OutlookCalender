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

        private readonly ObservableCollection<SyncLog> _syncLogsInternal;
        private readonly IRepository _repository;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _applyFilterEnabled;
        private bool _undoFilterEnabled;
        private bool _deleteHistoryEnabled;

        public SyncHistoryViewModel(IRepository repository)
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
                    UpdateSyncLogCollection(syncLogs);
                });
   
            }
        }

        private void ApplyFilter()
        {
            _syncLogsInternal.Clear();
            Task.Run(async () =>
            {
                var syncLogs = await _repository.FindAll<SyncLog>(_ => _.StartDate <= _startDate && _.EndDate >= _endDate);
                 UpdateSyncLogCollection(syncLogs);
            }).ContinueWith((_) => UndoFilterEnabled = true);
   
        }
        
        private void DeleteHistory()
        {

        }

        private void DeleteLog(Guid id)
        {
            var syncLog = _syncLogsInternal.SingleOrDefault(_ => _.Id == id);
            _syncLogsInternal.Remove(syncLog);
            _repository.Delete(syncLog);
        }

        private void UpdateSyncLogCollection(IList<SyncLog> syncLogs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (syncLogs.Any())
                {
                    syncLogs.OrderByDescending(_ => _.SyncDate).ToList().ForEach(s => _syncLogsInternal.Add(s));
                }
                DeleteHistoryEnabled = _syncLogsInternal.Any();
            });
        }

        private void UndoFilter ()
        {
            _syncLogsInternal.Clear();
            Task.Run(async () =>
            {
                var syncLogs = await _repository.GetAll<SyncLog>();
                UpdateSyncLogCollection(syncLogs);
            }).ContinueWith((_) => UndoFilterEnabled = false);
   
        }
    }
}
