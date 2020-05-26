using OutlookCalender.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutlookCalender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(RouteNames.SearchDetails, typeof(SearchDetailPage));
        }
    }
}