using CoreServices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using Models;

namespace OutlookCalender.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISyncService _syncService;
        private readonly IRepository _repository;
        private string _loginhint;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _searchValue;
        private bool _loginHintEnabled;
        private ObservableCollection<SearchResult> _searchResultsInternal;
        private bool _searchResultListVisible;
        private SearchResult _selectedSearchResult;
        private Action<SearchResult> _showSearchDetailPage;

        public RelayCommand SyncCommand { get; }
        public string Loginhint { get { return _loginhint; } set { SetBackingField(ref _loginhint, value, OnLoginhintChanged); } }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value); } }
        public string SearchValue { get { return _searchValue; } set { SetBackingField(ref _searchValue, value, OnSearchValueChanged); } }
        public bool LoginHintEnabled { get { return _loginHintEnabled; } private set { SetBackingField(ref _loginHintEnabled, value); } }
        public ReadOnlyObservableCollection<SearchResult> SearchResults { get; }
        public bool SearchResultListVisible { get { return _searchResultListVisible; } private set { SetBackingField(ref _searchResultListVisible, value); } }
        public RelayCommand SearchCommand { get; }
        
        public SearchResult SelectedSearchResult { get { return _selectedSearchResult; } set { SetBackingField(ref _selectedSearchResult, value, OnSelectedSearchResultChanged); } }

        public MainViewModel(ISyncService syncService, Action<SearchResult> showSearchDetailPage, IRepository repository)
        {
            _showSearchDetailPage = showSearchDetailPage;
            _syncService = syncService;
            _repository = repository;
            SyncCommand = new RelayCommand(OnSyncCommand)
            {
                IsEnabled = false
            };
            StartDate = DateTime.Today.AddDays(-7);
            EndDate = DateTime.Today.AddDays(30);
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

        private void OnSearchCommand()
        {
            _searchResultsInternal.Clear();
            if (string.IsNullOrWhiteSpace(_searchValue)) SearchResultListVisible = false;
            else
            {

                var searchValue = _searchValue.ToLower();
                Task.Run(async () =>
                  {
                      var events = await _repository.FindAll<EventModel>((e) => !string.IsNullOrEmpty(e.SearchMatch(searchValue).Item2));
                      if(events != null)
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                                SearchResultListVisible = events.Any();
                                if (_searchResultListVisible)
                                {
                                    events.OrderByDescending(e => e.Start).ToList().ForEach(_ => _searchResultsInternal.Add(SearchResult.FromEvent(_, searchValue)));
                                }
                          });
                      }
                  });
    
            }

        }

        private void OnSelectedSearchResultChanged(SearchResult oldvalue)
        {
            if (_selectedSearchResult != null) _showSearchDetailPage(_selectedSearchResult);
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
                Task.Run(async () =>
                {
                   await _syncService.Sync(_loginhint, _startDate, _endDate);

                }).ContinueWith((_) =>  OnSyncDone());
                
            }

        }
    }
}
