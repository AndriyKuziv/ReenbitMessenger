using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetFullGroupChatQuery : IQuery<GroupChat>
    {
        public Guid ChatId { get; }

        public GetFullGroupChatQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
