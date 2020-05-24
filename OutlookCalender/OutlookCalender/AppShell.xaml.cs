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
            Routing.RegisterRoute("SearchDetails", typeof(SearchDetailPage));
        }
    }
}