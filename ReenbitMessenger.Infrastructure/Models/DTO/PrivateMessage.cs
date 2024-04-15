using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class PrivateMessage
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string SentTime { get; set; }

        public long? MessageToReplyId { get; set; }
    }
}
