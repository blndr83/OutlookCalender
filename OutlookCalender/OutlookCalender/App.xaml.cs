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
        private ViewModelLocator _viewModelLocator;
        public static object UiParent { set { UiParentProvider.UiParent = value; } }

        public App()
        {
            _viewModelLocator = new ViewModelLocator(ContainerConfig.Configurate(ShowSearchDetailsPage));
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage { BindingContext = _viewModelLocator.GetViewModel<MainViewModel>() });        
        }

        private async void ShowSearchDetailsPage(SearchResult searchResult)
        {
            await ((NavigationPage)MainPage).PushAsync(new SearchDetailPage { BindingContext = searchResult });
        }


    }
}
