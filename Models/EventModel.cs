using System;

namespace Models
{
    public class EventModel : Entity
    {
        public  string Id { get; set; }
        public  DateTime Start { get; set; }
        public  DateTime End { get; set; }
        public  string Subject { get; set; }
        public  string BodyContent { get; set; }
        public string LocationDisplayName { get; set; }
        public string BodyContentWithoutHtml { get; set; }

        public  void Update(EventModel eventModel)
        {
            BodyContent = eventModel.BodyContent;
            Subject = eventModel.Subject;
            LocationDisplayName = eventModel.LocationDisplayName;
            BodyContentWithoutHtml = eventModel.BodyContentWithoutHtml;
        }
    }
}
