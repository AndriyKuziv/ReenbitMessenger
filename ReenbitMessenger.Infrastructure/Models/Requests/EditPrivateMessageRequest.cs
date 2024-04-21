using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class EditPrivateMessageRequest
    {
        public long MessageId { get; set; }
        public string Text { get; set; }
    }
}
