using System.ComponentModel.DataAnnotations;
namespace eventapp.Domain.Models
{
    public class Priority
    {
        public const int DefaultPriorityId = 1;
        [Key]
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
    }
}
