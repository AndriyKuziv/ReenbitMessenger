using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Models.Domain
{
    public class GroupChatMembers
    {
        public Guid Id { get; set; }
        public Guid GroupChatId { get; set; }
        public string UserId { get; set; }
        public short GroupChatRoleId { get; set; }
    }
}
