using CoreServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Common;
using System.Linq;

namespace OutlookCalender.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICalendarService _calendarService;
        private string _loginhint;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _searchValue;
        private bool _loginHintEnabled;

        public RelayCommand SyncCommand { get; }
        public string Loginhint { get { return _loginhint; } set { SetBackingField(ref _loginhint, value, OnLoginhintChanged); } }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value); } }
        public string SearchValue { get { return _searchValue; } set { SetBackingField(ref _searchValue, value, OnSearchValueChanged); } }
        public bool LoginHintEnabled { get { return _loginHintEnabled; } private set { SetBackingField(ref _loginHintEnabled, value); } }

        public MainViewModel(ICalendarService calendarService)
        {
            _calendarService = calendarService;
            SyncCommand = new RelayCommand(OnSyncCommand)
            {
                IsEnabled = false
            };
            StartDate = DateTime.Today.AddDays(-7);
            EndDate = DateTime.Today.AddDays(30);
            _calendarService.SyncDone = OnSyncDone;
            LoginHintEnabled = true;
        }

        private void OnSyncDone()
        {
            Device.BeginInvokeOnMainThread(() => {
                SyncCommand.IsEnabled = true;
                LoginHintEnabled = true;
            });
        }

        private void OnSearchValueChanged(string oldValue)
        {
           var events =  _calendarService.GetEventModels((e) => e.Subject.Contains(_searchValue)
            || e.BodyContent.RemoveHtmlTags().Contains(_searchValue));
            if(events.Any())
            {
                
            }
        }



        private void OnLoginhintChanged(string oldvalue)
        {
            SyncCommand.IsEnabled = !string.IsNullOrWhiteSpace(_loginhint);
        }

        private void OnSyncCommand()
        {
            if(!string.IsNullOrWhiteSpace(_loginhint))
            {
                SyncCommand.IsEnabled = false;
                LoginHintEnabled = false;
                Task.Run( () =>
                {
                     _calendarService.Sync(_loginhint, _startDate, _endDate);
           
                });
                
            }

        }
    }
}
