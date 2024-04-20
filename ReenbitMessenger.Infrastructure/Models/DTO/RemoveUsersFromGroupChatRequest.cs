using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class RemoveUsersFromGroupChatRequest
    {
        public IEnumerable<string> UsersIds { get; set; }
    }
}
