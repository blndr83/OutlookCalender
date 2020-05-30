using OutlookCalender.Locator;
using OutlookCalender.ViewModels;
using Xamarin.Forms.Xaml;

namespace OutlookCalender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly ActivityPopupViewModel _viewModel;

        public ActivityPopup()
        {
            _viewModel = ViewModelLocator.Instance.ActivityPopupViewModel;
            BindingContext = _viewModel;
            InitializeComponent();
            Indicator.IsRunning = false;
            IndicatorText.Text = string.Empty;
        }

        protected override void OnAppearing()
        {
            Indicator.IsRunning = true;
            IndicatorText.Text = _viewModel.Text; 
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
             Indicator.IsRunning = false;
            base.OnDisappearing();
        }
    }
}