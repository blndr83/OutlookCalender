using Models;
using System;
using System.Linq;
using Common;

namespace OutlookCalender.ViewModels
{
    public class SearchResult
    {
        public string Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public string Subject { get; private set; }
        public string BodyContent { get; private set; }
        public string LocationDisplayName { get; private set; }
        public string SearchMatch { get; private set; }
        public string SearchMatchLabel { get; private set; }

        public static SearchResult FromEvent(EventModel @event, string searchValue)
        {
            var searchResult = new SearchResult();
            var eventProperities = @event.GetType().GetProperties();
            var searchResultProperties = searchResult.GetType().GetProperties();
            eventProperities.ToList().ForEach(_ => searchResultProperties.FirstOrDefault(s => s.Name.Equals(_.Name))?.SetValue(searchResult, _.GetValue(@event)));
            var searchMatch = @event.SearchMatch(searchValue);

            if (searchMatch.Item1.Equals(nameof(BodyContent))) SetBodyContentSearchMatch(searchResult, searchValue);
            else searchResult.SearchMatch = searchMatch.Item2;

            SetSearchMatchLabel(searchResult, searchMatch.Item1);

            return searchResult;
        }

        private static void SetSearchMatchLabel(SearchResult searchResult, string searchMatchPropertyName)
        {
            if (searchMatchPropertyName.Equals(nameof(Subject)))
                searchResult.SearchMatchLabel = nameof(Subject);
            else if (searchMatchPropertyName.Equals(nameof(LocationDisplayName)))
                searchResult.SearchMatchLabel = "Location";
            else
                searchResult.SearchMatchLabel = "Body Content";

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
                    searchResult.SearchMatch = $"{threeDots}{bodyContentWithoutHtml.Substring(startIndex, length - startIndex)}{threeDotsAtTheEnd}";
                }
            }
        }
        
    }
}
