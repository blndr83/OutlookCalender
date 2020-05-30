using CoreServices;
using OutlookCalender.Constants;
using OutlookCalender.Locator;
using OutlookCalender.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OutlookCalender
{
    public partial class App : Application
    {
        public static object UiParent { set { UiParentProvider.UiParent = value; } }
        private readonly ActivityPopup _activityPopup;

        public App()
        {
            SQLitePCL.Batteries_V2.Init();
            ViewModelLocator.CreateInstance(ContainerConfig.Configurate(ShowSearchDetailsPage, ShowAlert, ShowActivityPopup, RemoveActivityPopup));
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

        private Task ShowActivityPopup(string text)
        {
            ViewModelLocator.Instance.ActivityPopupViewModel.Text = text;
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(_activityPopup);
        }

        private Task RemoveActivityPopup()
        {
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(_activityPopup);
        }

    }
}
