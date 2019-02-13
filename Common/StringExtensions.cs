using System;
using System.Text.RegularExpressions;

namespace Common
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string stringValue)
        {
            return Regex.Replace(stringValue, "<.*?>", String.Empty);
        }
        
    }
}
