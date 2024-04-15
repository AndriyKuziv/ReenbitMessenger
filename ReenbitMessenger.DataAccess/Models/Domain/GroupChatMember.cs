using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class GroupChatMember
    {
        public long Id { get; set; }
        public string GroupChatId { get; set; }
        public string UserId { get; set; }
        public byte GroupChatRoleId { get; set; }


        // Navigation properties
        public GroupChat? GroupChat { get; set; }
        public IdentityUser? User { get; set; }
        public GroupChatRole? Role { get; set; }
    }
}
