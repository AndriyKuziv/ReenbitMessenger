using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class AddUsersToGroupChatRequest
    {
        public IEnumerable<string> UsersIds { get; set; }
    }
}
