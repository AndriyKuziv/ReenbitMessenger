using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Data
{
    public class ChatsDataContext : DbContext
    {
        public ChatsDataContext(DbContextOptions<ChatsDataContext> options) : base(options) { }

        public DbSet<GroupChat> GroupChat { get; set; }
        public DbSet<GroupChatMember> GroupChatMember { get; set; }
        public DbSet<GroupChatMessage> GroupChatMessage { get; set; }
        public DbSet<GroupChatRole> GroupChatRole { get; set; }

        public DbSet<PrivateMessage> PrivateMessage { get; set; }
    }
}
