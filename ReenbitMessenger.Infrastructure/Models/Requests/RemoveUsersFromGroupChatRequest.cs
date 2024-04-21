using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class RemoveUsersFromGroupChatRequest
    {
        public IEnumerable<string> UsersIds { get; set; }
    }
}
