﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class DeleteMessagesFromGroupChatRequest
    {
        public IEnumerable<long> MessagesIds { get; set; }
    }
}
