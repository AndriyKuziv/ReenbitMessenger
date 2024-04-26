using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class GetMessagesHistoryRequest
    {
        public int NumberOfMessages { get; set; }
        public int Page { get; set; }
        public string ValueContains { get; set; } = "";
        public bool Ascending { get; set; } = true;
        public string OrderBy { get; set; }
    }
}
