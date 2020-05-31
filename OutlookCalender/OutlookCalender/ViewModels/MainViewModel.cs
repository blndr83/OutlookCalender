using CoreServices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using Models;
using Xamarin.Essentials;
using Common;
using MvvmHelpers;
using System.Collections.Generic;

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
        private readonly ObservableRangeCollection<SearchResult> _searchResultsInternal;
        private string _internetConnection;
        private readonly IUiService _uiService;
 

        public RelayCommand SyncCommand { get; }
        public Command<SearchResult> SearchResultSelectionChangedCommand {get;}
        public string Loginhint { get { return _loginhint; } set { SetBackingField(ref _loginhint, value, OnLoginhintChanged); } }
        public DateTime StartDate { get { return _startDate; } set { SetBackingField(ref _startDate, value); } }
        public DateTime EndDate { get { return _endDate; } set { SetBackingField(ref _endDate, value); } }
        public string SearchValue { get { return _searchValue; } set { SetBackingField(ref _searchValue, value, OnSearchValueChanged); } }
        public bool LoginHintEnabled { get { return _loginHintEnabled; } private set { SetBackingField(ref _loginHintEnabled, value); } }
        public ReadOnlyObservableCollection<SearchResult> SearchResults { get; }
        public RelayCommand SearchCommand { get; }
        public string InternetConnection { get { return _internetConnection; } private set { SetBackingField(ref _internetConnection, value); } }
        public Action SearchResultListChanged { get; set; }

        public MainViewModel(ISyncService syncService, IRepository repository, IUiService uiService)
        {
            _syncService = syncService;
            _repository = repository;
            SyncCommand = new RelayCommand(OnSyncCommand)
            {
                IsEnabled = false
            };
            StartDate = DateTime.Today.AddDays(-7);
            EndDate = DateTime.Today.AddDays(30);
            LoginHintEnabled = true;
            _searchResultsInternal = new ObservableRangeCollection<SearchResult>();
            SearchResults = new ReadOnlyObservableCollection<SearchResult>(_searchResultsInternal);
            SearchCommand = new RelayCommand(OnSearchCommand);
            SetInternetConnection();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            SearchResultSelectionChangedCommand = new Command<SearchResult>(SearchResultSelectionChanged);
            _uiService = uiService;
            _syncService.AfterLogin =  () => Device.BeginInvokeOnMainThread(async () => await _uiService.ShowActivityPopup("downloading calendar entries ...")); 
        }

        private void SearchResultSelectionChanged(SearchResult item)
        {
             if (item != null) _uiService.ShowSearchResult(item);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            SetInternetConnection();
        }

        private void SetInternetConnection()
        {
            InternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet ? "🌐 Internet" : "Kein Internet";
            if (_loginHintEnabled) SetSyncCommandEnabled();
        }

        private void OnSyncDone()
        {
            Device.BeginInvokeOnMainThread( async () => {
                SyncCommand.IsEnabled = true;
                LoginHintEnabled = true;
                await _uiService.RemoveActivityPopup();
            });
        }

        private void OnSearchValueChanged(string oldValue)
        {
            if (string.IsNullOrWhiteSpace(_searchValue)) {
                _searchResultsInternal.Clear();
            }
        }

        private void OnSearchCommand()
        {
            SearchResultListChanged();
            _searchResultsInternal.Clear();
            if(!string.IsNullOrWhiteSpace(_searchValue))
            {

                var searchValue = _searchValue.ToLower();

                Task.Run(async () =>
                  {
                      var events = await _repository.FindAll(EventModelExtensions.GetSearchCondition(searchValue));
                      if(events != null)
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                                if (events.Any())
                                {
                                    var tempList = new List<SearchResult>();
                                    events.OrderByDescending(e => e.Start).ToList().ForEach(_ => tempList.Add(SearchResult.FromEvent(_, searchValue)));
                                  _searchResultsInternal.AddRange(tempList);
                                }
                          });
                      }
                  });
    
            }

        }

        private void OnLoginhintChanged(string oldvalue)
        {
            SetSyncCommandEnabled();
        }

        private void SetSyncCommandEnabled()
        {
            SyncCommand.IsEnabled = !string.IsNullOrWhiteSpace(_loginhint) && Connectivity.NetworkAccess == NetworkAccess.Internet;
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
