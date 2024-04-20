using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class AddUsersToGroupChatRequest
    {
        public IEnumerable<string> UsersIds { get; set; }
    }
}
