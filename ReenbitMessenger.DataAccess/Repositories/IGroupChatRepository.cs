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
        // Group chat
        Task<GroupChat> GetFullAsync(Guid chatId);
        Task<IEnumerable<GroupChat>> GetUserChatsAsync(string userId);

        // Group chat members
        Task<IEnumerable<GroupChatMember>> GetMembersAsync(Guid chatId);
        Task<bool> IsInGroupChat(Guid chatId, string userId);
        Task<GroupChatMember> AddUserToGroupChatAsync(GroupChatMember member);
        Task<GroupChatMember> RemoveUserFromGroupChatAsync(Guid chatId, string userId);

        // Group chat messages
        Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid chatId);
        Task<GroupChatMessage> GetMessageAsync(long messageId);
        Task<IEnumerable<GroupChatMessage>> GetMessageHistoryAsync(string userId);
        Task<GroupChatMessage> CreateGroupChatMessageAsync(GroupChatMessage groupChatMessage);
        Task<GroupChatMessage> DeleteGroupChatMessageAsync(Guid chatId, long messageId);
    }
}
