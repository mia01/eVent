using System;
using System.ComponentModel.DataAnnotations;

namespace eventapp.Domain.Models
{
    public class Event
    {
        [Key]
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Reminder { get; set; }
        public bool publicEvent { get; set; }
        public bool ReminderSent { get; set; }

    }
}
