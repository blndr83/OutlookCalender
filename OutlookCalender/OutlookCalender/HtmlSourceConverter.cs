using System;
using System.Globalization;
using Xamarin.Forms;

namespace OutlookCalender
{
    class HtmlSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var html = new HtmlWebViewSource();
            if (value != null && value is string) html.Html = System.Net.WebUtility.HtmlDecode(value as string);
            return html;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
