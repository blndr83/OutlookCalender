using System;
using System.Globalization;
using Xamarin.Forms;

namespace OutlookCalender
{
    public class DateTimeToDateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is DateTime)
            {
                var dateTimeValue = (DateTime)value;
                return dateTimeValue.ToShortDateString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
