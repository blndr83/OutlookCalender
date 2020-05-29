using System;

namespace OutlookCalender.ViewModels
{
    public class RelayCommand : RelayCommandBase
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            IsEnabled = true;
        }

        public override void Execute(object parameter)
        {
            _execute();
        }


    }
}
