using OutlookCalender.ViewModels;
using Xamarin.Forms;

namespace OutlookCalender
{
    public partial class MainPage : ContentPage
    {
        private VisualElement _previousSelected;
        private MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            _previousSelected = null;
            _viewModel.SearchResultListChanged = () => _previousSelected = null;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var nextSelected = sender as VisualElement;
            if (_previousSelected != null) VisualStateManager.GoToState(_previousSelected, "Normal");
            VisualStateManager.GoToState(nextSelected, "Selected");
            _previousSelected = nextSelected;
        }
    }
}
