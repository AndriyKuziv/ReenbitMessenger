﻿using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class PrivateMessage
    {
        public long Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public long? MessageToReplyId { get; set; }

        // Navigation properties
        public IdentityUser? SenderUser { get; set; }
        public IdentityUser? ReceiverUser { get; set; }
    }
}
