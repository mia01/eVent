using System;
using System.ComponentModel.DataAnnotations;

namespace eventapp.Models
{
    public class Task
    {
        [Key]
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public long PriorityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
