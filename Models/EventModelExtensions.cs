using Common;
using System;

namespace Models
{
    public static class EventModelExtensions
    {
        public static Tuple<string,string> SearchMatch(this EventModel eventModel, string searchValue)
        {
            searchValue = searchValue.ToLower();
            if (!string.IsNullOrEmpty(eventModel.Subject) && eventModel.Subject.ToLower().Contains(searchValue))
                return new Tuple<string, string>( nameof(EventModel.Subject), eventModel.Subject);
            if (!string.IsNullOrEmpty(eventModel.LocationDisplayName) && eventModel.LocationDisplayName.ToLower().Contains(searchValue))
                return new Tuple<string, string>(nameof(EventModel.LocationDisplayName), eventModel.LocationDisplayName);
            if (!string.IsNullOrEmpty(eventModel.BodyContent) && eventModel.BodyContent.RemoveHtmlTags().ToLower().Contains(searchValue))
                return new Tuple<string, string>(nameof(EventModel.BodyContent) , eventModel.BodyContent);
            return new Tuple<string, string>(null, null);
        }
    }
}
