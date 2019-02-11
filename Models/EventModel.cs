using System;

namespace Models
{
    public class EventModel : Entity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual string Subject { get; set; }
        public virtual string BodyContent { get; set; }

        public virtual void Update(EventModel eventModel)
        {
            BodyContent = eventModel.BodyContent;
        }
    }
}
