using CoreServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Common;
using System.Linq;
using System.Collections.ObjectModel;
using Models;

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
        private ObservableCollection<SearchResult> _searchResultsInternal;
        private bool _searchResultListVisible;

        public RelayCommand SyncCommand { get; }
        public string Loginhint { get { return _loginhint; } set { SetBackingField(ref _loginhint, value, OnLoginhintChanged); } }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value); } }
        public string SearchValue { get { return _searchValue; } set { SetBackingField(ref _searchValue, value, OnSearchValueChanged); } }
        public bool LoginHintEnabled { get { return _loginHintEnabled; } private set { SetBackingField(ref _loginHintEnabled, value); } }
        public ReadOnlyObservableCollection<SearchResult> SearchResults { get; }
        public bool SearchResultListVisible { get { return _searchResultListVisible; } private set { SetBackingField(ref _searchResultListVisible, value); } }
        public RelayCommand SearchCommand { get; }

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
            _searchResultsInternal = new ObservableCollection<SearchResult>();
            SearchResults = new ReadOnlyObservableCollection<SearchResult>(_searchResultsInternal);
            SearchCommand = new RelayCommand(OnSearchCommand);
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
            if (string.IsNullOrWhiteSpace(_searchValue)) {
                _searchResultsInternal.Clear();
                SearchResultListVisible = false;
            }
        }

        private async void OnSearchCommand()
        {
            _searchResultsInternal.Clear();
            if (string.IsNullOrWhiteSpace(_searchValue)) SearchResultListVisible = false;
            else
            {
                var searchValue = _searchValue.ToLower();
                var events = await _calendarService.GetEventModels((e) => (!string.IsNullOrEmpty(e.Subject) && e.Subject.ToLower().Contains(searchValue))
                || (!string.IsNullOrEmpty(e.BodyContent) && e.BodyContent.RemoveHtmlTags().ToLower().Contains(searchValue))
                || (!string.IsNullOrEmpty(e.LocationDisplayName) && e.LocationDisplayName.Contains(searchValue)));
                SearchResultListVisible = events.Any();

                if (_searchResultListVisible)
                {
                    events.OrderByDescending(e => e.Start).ToList().ForEach(_ => _searchResultsInternal.Add(SearchResult.FromEvent(_, searchValue)));

                }
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
