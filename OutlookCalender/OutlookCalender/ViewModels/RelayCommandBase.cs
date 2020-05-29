using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace OutlookCalender.ViewModels
{
    public abstract class RelayCommandBase : ObservableObject, ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetBackingField(ref _isEnabled, value, OnEnabledChanged); }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        private void OnEnabledChanged(bool oldValue)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public virtual void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
