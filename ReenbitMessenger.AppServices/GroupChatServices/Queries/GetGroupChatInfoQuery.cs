using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetGroupChatInfoQuery : IQuery<GroupChat>
    {
        public Guid ChatId { get; }

        public GetGroupChatInfoQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
