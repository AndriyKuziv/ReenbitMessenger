﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class SendMessageToGroupChatRequest
    {
        public string Text { get; set; }
        public long? MessageToReplyId { get; set; } = null;
    }
}
