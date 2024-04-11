using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class GroupChatMessages
    {
        public Guid Id { get; set; }
        public string SenderUserId { get; set; }
        public string GroupChatId { get; set; }
        public string Text { get; set; }
        public Guid MessageToReplyId { get; set; }
    }
}
