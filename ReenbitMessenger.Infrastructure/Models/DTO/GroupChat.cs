using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GroupChat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public List<GroupChatMember>? GroupChatMembers { get; set; }
        public List<GroupChatMessage>? GroupChatMessages { get; set; }
    }
}
