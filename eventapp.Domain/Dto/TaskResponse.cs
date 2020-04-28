using System;

namespace eventapp.Domain.Dto
{
    public class TaskResponse
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public long PriorityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUsername { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToUsername { get; set; }
        public bool Reminder { get; set; }
        public bool ReminderSent { get; set; }
    }
}
