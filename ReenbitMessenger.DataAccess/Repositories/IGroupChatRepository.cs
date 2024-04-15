using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat, Guid>
    {
        Task<GroupChat> GetFullAsync(Guid chatId);
        Task<IEnumerable<GroupChat>> GetUserChatsAsync(string userId);
        Task<IEnumerable<GroupChatMessage>> GetMessageHistoryAsync(string userId);

        Task<IEnumerable<GroupChatMember>> GetMembersAsync(Guid id);
        Task<bool> IsInGroupChat(Guid chatId, string userId);
        Task<GroupChatMember> AddUserToGroupChatAsync(GroupChatMember member);
        Task<GroupChatMember> RemoveUserFromGroupChatAsync(Guid chatId, string userId);

        Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid id);
        Task<GroupChatMessage> CreateGroupChatMessageAsync(GroupChatMessage groupChatMessage);
        Task<GroupChatMessage> DeleteGroupChatMessageAsync(Guid chatId, long messageId);
    }
}
