﻿using ReenbitMessenger.DataAccess.Models.Domain;
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
        Task<GroupChat> GetInfoAsync(Guid chatId);
        Task<IEnumerable<GroupChat>> GetUserChatsAsync(string userId,
            string valueContains = "", int startAt = 0, int take = 20,
            bool ascending = true);

        // Group chat members
        Task<GroupChatMember> GetMemberAsync(long memberId);
        Task<IEnumerable<GroupChatMember>> FilterMembersAsync(Func<GroupChatMember, bool> predicate);
        Task<IEnumerable<GroupChatMember>> GetGroupChatMembersAsync(Guid chatId);
        bool IsInGroupChat(Guid chatId, string userId);
        Task<GroupChatMember> AddUserToGroupChatAsync(GroupChatMember member);
        Task<GroupChatMember> RemoveUserFromGroupChatAsync(Guid chatId, string userId);

        // Group chat messages
        Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid chatId);
        Task<GroupChatMessage> GetMessageAsync(long messageId);
        Task<IEnumerable<GroupChatMessage>> FilterMessagesAsync(Func<GroupChatMessage, bool> predicate,
            int startAt = 0, int take = 20, bool ascending = true);
        Task<IEnumerable<GroupChatMessage>> GetMessageHistoryAsync(string userId,
            int startAt = 0, int take = 20, string valueContains = "",
            bool ascending = true, string orderBy = "SentTime");
        Task<GroupChatMessage> CreateGroupChatMessageAsync(GroupChatMessage groupChatMessage);
        Task<GroupChatMessage> DeleteGroupChatMessageAsync(Guid chatId, long messageId);
    }
}
