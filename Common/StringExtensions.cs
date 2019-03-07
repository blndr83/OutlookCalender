using System;
using System.Text.RegularExpressions;

namespace Common
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string stringValue)
        {
            var text = System.Net.WebUtility.HtmlDecode(stringValue);
            return Regex.Replace(text, "<.*?>", String.Empty);
        }
        
    }
}
