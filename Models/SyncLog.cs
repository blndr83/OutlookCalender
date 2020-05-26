using System;

namespace Models
{
    public class SyncLog : Entity
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AmountOfAddedItems { get; set; }
        public int AmountOfUpdatedItems { get; set; }
        public DateTime SyncDate { get; set; }
    }
}
