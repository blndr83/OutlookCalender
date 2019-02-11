using System;
using System.Windows.Input;

namespace OutlookCalender.ViewModels
{
    public class RelayCommand : ObservableObject, ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _isEnabled;

        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetBackingField(ref _isEnabled, value, OnEnabledChanged); }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        private void OnEnabledChanged(bool oldValue)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
