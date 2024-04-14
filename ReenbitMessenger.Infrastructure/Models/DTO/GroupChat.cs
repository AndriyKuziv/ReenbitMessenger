using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GroupChat
    {
        public string Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public IEnumerable<GroupChatMember>? GroupChatMembers { get; set; }
        public IEnumerable<GroupChatMessage>? GroupChatMessages { get; set; }
    }
}
