using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat>
    {
        Task<IEnumerable<GroupChatMember>> GetMembersAsync(Guid id);
        Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid id);
    }
}
