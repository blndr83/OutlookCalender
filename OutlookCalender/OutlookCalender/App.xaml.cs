using CoreServices;
using OutlookCalender.Locator;
using OutlookCalender.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OutlookCalender
{
    public partial class App : Application
    {
        public static object UiParent { set { UiParentProvider.UiParent = value; } }

        public App()
        {
            SQLitePCL.Batteries_V2.Init();
            ViewModelLocator.CreateInstance(ContainerConfig.Configurate(ShowSearchDetailsPage));
            InitializeComponent();
            MainPage = new AppShell();
        }

        private async void ShowSearchDetailsPage(SearchResult searchResult)
        {
            ViewModelLocator.Instance.SearchResult = searchResult;
            await Shell.Current.GoToAsync("SearchDetails");
        }


    }
}
