using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Data
{
    public class MessengerDataContext : IdentityDbContext
    {
        public MessengerDataContext(DbContextOptions<MessengerDataContext> dbContextOptions) : base(dbContextOptions) { }

        public virtual DbSet<GroupChat> GroupChat { get; set; }
        public virtual DbSet<GroupChatMember> GroupChatMember { get; set; }
        public virtual DbSet<GroupChatMessage> GroupChatMessage { get; set; }
        public virtual DbSet<GroupChatRole> GroupChatRole { get; set; }

        public virtual DbSet<PrivateMessage> PrivateMessage { get; set; }

        public virtual DbSet<UserAvatar> UserAvatar { get; set; }
    }
}
