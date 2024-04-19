using ReenbitMessenger.Infrastructure.Models.DTO;
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
        Task<GroupChat> GetFullGroupChatAsync(string chatId);
        Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync();

        Task<IEnumerable<PrivateMessage>> GetUserPrivateMessagesHistoryAsync();

        Task<bool> SendMessageToGroupChatAsync(string chatId, SendMessageToGroupChatRequest sendGroupChatMessageRequest);

        Task<bool> SendPrivateMessageAsync(SendPrivateMessageRequest sendPrivateMessageRequest);

        Task<bool> AddUsersToGroupChatAsync(string chatId, AddUsersToGroupChatRequest addUserToGroupChatRequest);
    }
}
