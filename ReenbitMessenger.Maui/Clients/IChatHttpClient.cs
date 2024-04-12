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
        Task<IEnumerable<GroupChat>> GetUsersGroupChats(string userId);

        Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistory(string userId);

        Task<IEnumerable<PrivateMessage>> GetUserPrivateMessagesHistory(string userId);

        Task<GroupChatMessage> SendMessageToGroupChat(SendGroupChatMessageRequest sendGroupChatMessageRequest);

        Task<PrivateMessage> SendPrivateMessage(SendPrivateMessageRequest sendPrivateMessageRequest);
    }
}
