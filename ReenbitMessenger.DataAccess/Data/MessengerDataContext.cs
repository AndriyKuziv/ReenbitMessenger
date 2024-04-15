using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Data
{
    public class MessengerDataContext : IdentityDbContext
    {
        public MessengerDataContext(DbContextOptions<MessengerDataContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<GroupChat> GroupChat { get; set; }
        public DbSet<GroupChatMember> GroupChatMember { get; set; }
        public DbSet<GroupChatMessage> GroupChatMessage { get; set; }
        public DbSet<GroupChatRole> GroupChatRole { get; set; }

        public DbSet<PrivateMessage> PrivateMessage { get; set; }
    }
}
