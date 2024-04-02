using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Library.Models.DTO
{
    public class AddUsersToGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
