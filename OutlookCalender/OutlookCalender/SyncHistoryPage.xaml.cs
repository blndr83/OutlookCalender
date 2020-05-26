using OutlookCalender.Locator;
using OutlookCalender.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutlookCalender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncHistoryPage : ContentPage
    {
        private readonly SyncHistoryViewModel _viewModel;

        public SyncHistoryPage()
        {
            _viewModel = ViewModelLocator.Instance.GetViewModel<SyncHistoryViewModel>();
            BindingContext = _viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.InitSyncLogCollection();
        }
    }
}