using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class SendPrivateMessageRequest
    {
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public long? MessageToReplyId { get; set; } = null;
    }
}
