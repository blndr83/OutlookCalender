namespace OutlookCalender.ViewModels
{
    public class ActivityPopupViewModel : ViewModelBase
    {
        public string Text { get { return _text; } set { SetBackingField(ref _text, value); } }
        private string _text;
    }
}
