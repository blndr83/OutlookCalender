using OutlookCalender.Locator;
using Xamarin.Forms;

namespace OutlookCalender
{
	public partial class SearchDetailPage : ContentPage
	{
		public SearchDetailPage ()
		{
			BindingContext = ViewModelLocator.Instance.SearchResult;
			InitializeComponent ();
		}
	}
}