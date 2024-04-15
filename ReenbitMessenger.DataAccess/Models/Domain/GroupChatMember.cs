using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class GroupChatMember
    {
        public long Id { get; set; }
        public Guid GroupChatId { get; set; }
        public string UserId { get; set; }
        public byte GroupChatRoleId { get; set; }

        // Navigation properties
        public GroupChat? GroupChat { get; set; }
        public IdentityUser? User { get; set; }
        public GroupChatRole? Role { get; set; }
    }
}
