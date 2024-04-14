using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GroupChatMember
    {
        public long Id { get; set; }
        public string GroupChatId { get; set; }
        public string UserId { get; set; }
        public short GroupChatRoleId { get; set; }

        // Navigation properties
        public GroupChatRole? GroupChatRole { get; set; }
    }
}
