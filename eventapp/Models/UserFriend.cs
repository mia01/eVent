using System.ComponentModel.DataAnnotations;

namespace eventapp.Models
{
    public class UserFriend
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string UserFriendId { get; set; }
        public bool Accepted { get; set; }
    }
}
