using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OutlookCalender.ViewModels
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetBackingField<T>(ref T currentValue, T value, Action<T> onChanged = null ,[CallerMemberName] string propertyName = null)
        {
            if (Equals(currentValue, value)) return;

            var oldValue = currentValue;
            currentValue = value;

            OnPropertyChanged(propertyName);

            onChanged?.Invoke(oldValue);
       }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
