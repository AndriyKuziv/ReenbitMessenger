using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Library.Models.DTO
{
    public class RemoveUsersFromGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
