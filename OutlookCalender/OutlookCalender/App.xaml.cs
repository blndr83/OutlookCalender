using CoreServices;
using OutlookCalender.Constants;
using OutlookCalender.Locator;
using OutlookCalender.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OutlookCalender
{
    public partial class App : Application, IUiService
    {
        public static object UiParent { set { UiParentProvider.UiParent = value; } }

        public Action<SearchResult> ShowSearchResult { get; }

        public Func<string, Task<bool>> DisplayAlert { get; }

        public Func<string, Task> ShowActivityPopup { get; }

        public Func<Task> RemoveActivityPopup { get; }

        private readonly ActivityPopup _activityPopup;

        public App()
        {
            SQLitePCL.Batteries_V2.Init();
            ShowSearchResult = ShowSearchDetailsPage;
            DisplayAlert = ShowAlert;
            RemoveActivityPopup = OnRemoveActivityPopup;
            ShowActivityPopup = OnShowActivityPopup;
            ViewModelLocator.CreateInstance(ContainerConfig.Configurate(this));
            InitializeComponent();
            MainPage = new AppShell();
            _activityPopup = new ActivityPopup();
        }

        private async void ShowSearchDetailsPage(SearchResult searchResult)
        {
            ViewModelLocator.Instance.SearchResult = searchResult;
            await Shell.Current.GoToAsync(RouteNames.SearchDetails);
        }

        private Task<bool> ShowAlert(string message)
        {
            return Shell.Current.DisplayAlert("", message, "Yes", "No");
        }

        private Task OnShowActivityPopup(string text)
        {
            ViewModelLocator.Instance.ActivityPopupViewModel.Text = text;
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(_activityPopup);
        }

        private Task OnRemoveActivityPopup()
        {
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(_activityPopup);
        }

    }
}
