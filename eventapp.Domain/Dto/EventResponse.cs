using System;

namespace eventapp.Domain.Dto
{
    public class EventResponse
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Reminder { get; set; }
        public bool PublicEvent { get; set; }
        public bool ReminderSent { get; set; }
    }
}
