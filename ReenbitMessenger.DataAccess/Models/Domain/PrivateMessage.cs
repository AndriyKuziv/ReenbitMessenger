using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class PrivateMessage
    {
        public long Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Text { get; set; }
        public string SentTime { get; set; }
        public string? MessageToReplyId { get; set; }
    }
}
