using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Common;

namespace OutlookCalender.ViewModels
{
    public class SearchResult
    {
        public Guid Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public string Subject { get; private set; }
        public string BodyContent { get; private set; }
        public string BodyContentSearchMatch { get; private set; }

        public static SearchResult FromEvent(EventModel @event, string searchValue)
        {
            var searchResult = new SearchResult();
            var eventProperities = @event.GetType().GetProperties();
            var searchResultProperties = searchResult.GetType().GetProperties();
            eventProperities.ToList().ForEach(_ => searchResultProperties.FirstOrDefault(s => s.Name.Equals(_.Name))?.SetValue(searchResult, _.GetValue(@event)));
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
                    for (var i = startIndex; i > 0; i--)
                    {
                        if (bodyContentWithoutHtml[i].Equals(' '))
                        {
                            startIndex = i;
                            break;
                        }
                    }
                    var length = startIndex + 60 < bodyContentWithoutHtml.Length - 1 ? startIndex + 60 : bodyContentWithoutHtml.Length - 1;
                    var threeDots = startIndex > 0 ? "..." : string.Empty;
                    searchResult.BodyContentSearchMatch = $"{threeDots}{bodyContentWithoutHtml.Substring(startIndex, length - startIndex)}...";
                }
            }
        }
    }
}
