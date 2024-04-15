using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class GroupChatMessage
    {
        public long Id { get; set; }
        public string SenderUserId { get; set; }
        public Guid GroupChatId { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public long? MessageToReplyId { get; set; }

        // Navigation properties
        //public IdentityUser? User { get; set; }
    }
}
