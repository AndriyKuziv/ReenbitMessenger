using System;
using System.Collections.Generic;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class RemoveUsersFromGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
