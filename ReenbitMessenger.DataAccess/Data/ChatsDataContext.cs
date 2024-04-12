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

        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<GroupChatMember> GroupChatMembers { get; set; }
        public DbSet<GroupChatMessage> GroupChatMessages { get; set; }

        public DbSet<PrivateMessage> PrivateMessages { get; set; }
    }
}
