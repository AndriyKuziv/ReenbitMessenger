using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IChatHttpClient
    {
        Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync(GetGroupChatsRequest getChatsRequest);
        Task<IEnumerable<GroupChatMessage>> GetMessagesHistory(GetMessagesHistoryRequest getMessagesRequest);
        Task<bool> CreateGroupChatAsync(CreateGroupChatRequest createChatRequest);
        Task<bool> JoinGroupChatAsync(string chatId);
        Task<bool> LeaveGroupChatAsync(string chatId);
        Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync();
    }
}
