using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GroupChatMessage
    {
        public long Id { get; set; }
        public string SenderId { get; set; }
        public Guid GroupChatId { get; set; }
        public DateTime SentTime { get; set; }
        public string Text { get; set; }
    }
}
