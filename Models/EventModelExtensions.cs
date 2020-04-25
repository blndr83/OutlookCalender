using System;
using System.Linq.Expressions;

namespace Models
{
    public static class EventModelExtensions
    {
        public static Expression<Func<EventModel, bool>> GetSearchCondition(string searchValue)
        {
            return (e) => (!string.IsNullOrEmpty(e.Subject) && e.Subject.ToLower().Contains(searchValue)) || (!string.IsNullOrEmpty(e.LocationDisplayName) && e.LocationDisplayName.ToLower().Contains(searchValue)) || (!string.IsNullOrEmpty(e.BodyContentWithoutHtml) && e.BodyContentWithoutHtml.ToLower().Contains(searchValue));
        }

        public static Tuple<string,string> SearchMatch(this EventModel eventModel, string searchValue)
        {
            searchValue = searchValue.ToLower();
            if (!string.IsNullOrEmpty(eventModel.Subject) && eventModel.Subject.ToLower().Contains(searchValue))
                return new Tuple<string, string>( nameof(EventModel.Subject), eventModel.Subject);
            if (!string.IsNullOrEmpty(eventModel.LocationDisplayName) && eventModel.LocationDisplayName.ToLower().Contains(searchValue))
                return new Tuple<string, string>(nameof(EventModel.LocationDisplayName), eventModel.LocationDisplayName);
            if (!string.IsNullOrEmpty(eventModel.BodyContentWithoutHtml) && eventModel.BodyContentWithoutHtml.ToLower().Contains(searchValue))
                return new Tuple<string, string>(nameof(EventModel.BodyContent) , eventModel.BodyContentWithoutHtml);
            return new Tuple<string, string>(null, null);
        }
    }
}
