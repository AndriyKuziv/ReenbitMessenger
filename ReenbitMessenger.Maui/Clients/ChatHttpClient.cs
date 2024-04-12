using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Clients
{
    public class ChatHttpClient : IChatHttpClient
    {
        public Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistory(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PrivateMessage>> GetUserPrivateMessagesHistory(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GroupChat>> GetUsersGroupChats(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<GroupChatMessage> SendMessageToGroupChat(SendGroupChatMessageRequest sendGroupChatMessageRequest)
        {
            throw new NotImplementedException();
        }

        public Task<PrivateMessage> SendPrivateMessage(SendPrivateMessageRequest sendPrivateMessageRequest)
        {
            throw new NotImplementedException();
        }
    }
}
