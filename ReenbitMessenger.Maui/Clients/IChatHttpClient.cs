﻿using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IChatHttpClient
    {
        Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync();
        Task<bool> CreateGroupChatAsync(CreateGroupChatRequest createChatRequest);
        Task<bool> DeleteGroupChatAsync(string chatId);
        Task<GroupChat> GetFullGroupChatAsync(string chatId);
        Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync();

        Task<bool> SendMessageToGroupChatAsync(string chatId, SendMessageToGroupChatRequest sendGroupChatMessageRequest);

        Task<bool> AddUsersToGroupChatAsync(string chatId, AddUsersToGroupChatRequest addUserToGroupChatRequest);
    }
}
