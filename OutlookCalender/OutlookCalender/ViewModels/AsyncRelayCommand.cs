
using MvvmHelpers;
using System;
using System.Threading.Tasks;

namespace OutlookCalender.ViewModels
{
    public class AsyncRelayCommand : RelayCommandBase
    {
        private readonly Func<Task> _execute;

        public AsyncRelayCommand(Func<Task> execute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            IsEnabled = true;
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute(null))
            {
                try
                {
                    IsEnabled = false;
                    await _execute();
                }
                finally
                {
                    IsEnabled = true;
                }
            }
        }

        public override void Execute(object parameter)
        {
            ExecuteAsync().SafeFireAndForget();
        }
    }
}
