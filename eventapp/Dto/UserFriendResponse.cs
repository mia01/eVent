﻿using System.ComponentModel.DataAnnotations;

namespace eventapp.Models
{
    public class UserFriendResponse
    {
        public long Id { get; set; }
        public string FriendId { get; set; }
        public string FriendUsername { get; set; }
        public bool Accepted { get; set; }
    }
}
