using CoreServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OutlookCalender.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICalendarService _calendarService;
        private string _loginhint;

        public RelayCommand SyncCommand { get; }
        public string Loginhint { get { return _loginhint; } set { SetBackingField(ref _loginhint, value, OnLoginhintChanged); } }
      
        public MainViewModel(ICalendarService calendarService)
        {
            _calendarService = calendarService;
            SyncCommand = new RelayCommand(OnSyncCommand)
            {
                IsEnabled = false
            };
        }

        private void OnLoginhintChanged(string oldvalue)
        {
            SyncCommand.IsEnabled = !string.IsNullOrWhiteSpace(_loginhint);
        }

        private void OnSyncCommand()
        {
            if(!string.IsNullOrWhiteSpace(_loginhint))
            {
                Task.Run( () =>
                {
                     _calendarService.Sync(_loginhint, DateTime.Today.AddDays(-7), DateTime.Today.AddDays(30));
     

                });
            }

        }
    }
}
