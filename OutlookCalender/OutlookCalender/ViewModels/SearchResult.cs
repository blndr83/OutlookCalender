using Models;
using System;
using System.Linq;
using Common;
using Xamarin.Forms;

namespace OutlookCalender.ViewModels
{
    public class SearchResult
    {
        public string Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public string Subject { get; private set; }
        public string BodyContent { get; private set; }
        public HtmlWebViewSource BodyContentWebView { get; private set; }
        public string LocationDisplayName { get; private set; }
        public string BodyContentSearchMatch { get; private set; }

        public static SearchResult FromEvent(EventModel @event, string searchValue)
        {
            var searchResult = new SearchResult();
            var eventProperities = @event.GetType().GetProperties();
            var searchResultProperties = searchResult.GetType().GetProperties();
            eventProperities.ToList().ForEach(_ => searchResultProperties.FirstOrDefault(s => s.Name.Equals(_.Name))?.SetValue(searchResult, _.GetValue(@event)));
            searchResult.BodyContentWebView = new HtmlWebViewSource { Html = System.Net.WebUtility.HtmlDecode(@event.BodyContent) };
            SetBodyContentSearchMatch(searchResult, searchValue);

            return searchResult;
        }

        private static void SetBodyContentSearchMatch(SearchResult searchResult, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchResult.BodyContent))
            {
                var bodyContentWithoutHtml = searchResult.BodyContent.RemoveHtmlTags().Replace(Environment.NewLine, "");
                if (bodyContentWithoutHtml.ToLower().Contains(searchValue))
                {
                    var indexOfSearchMatch = bodyContentWithoutHtml.ToLower().IndexOf(searchValue);
                    var startIndex = indexOfSearchMatch - 20 < 0 ? indexOfSearchMatch : indexOfSearchMatch - 20;
                    if (startIndex < 20) startIndex = 0;
                    bool condition(char c) { return c.Equals(' ') || c.Equals('.') || c.Equals(',') || c.Equals(';') || c.Equals(':'); }
                    for (var i = startIndex; i > 0; i--)
                    {
                        if (condition(bodyContentWithoutHtml[i]))
                        {
                            startIndex = i;
                            break;
                        }
                    }
                    var length = startIndex + 40 < bodyContentWithoutHtml.Length - 1 ? startIndex + 40 : bodyContentWithoutHtml.Length - 1;
                    for(var i=length; i < bodyContentWithoutHtml.Length; i++)
                    {
                        if (condition(bodyContentWithoutHtml[i]))
                        {
                            length = i;
                            break;
                        }
                    }

                    var threeDots = startIndex > 0 ? "..." : string.Empty;
                    var threeDotsAtTheEnd = length < bodyContentWithoutHtml.Length - 1 ? "..." : string.Empty;
                    searchResult.BodyContentSearchMatch = $"{threeDots}{bodyContentWithoutHtml.Substring(startIndex, length - startIndex)}{threeDotsAtTheEnd}";
                }
            }
        }
        
    }
}
