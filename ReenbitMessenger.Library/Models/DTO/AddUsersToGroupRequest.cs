using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class AddUsersToGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
