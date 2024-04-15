namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GroupChatMember
    {
        public long Id { get; set; }
        public Guid GroupChatId { get; set; }
        public string UserId { get; set; }
        public short GroupChatRoleId { get; set; }

        // Navigation properties
        public GroupChatRole? GroupChatRole { get; set; }
        public User? User { get; set; }
    }
}
