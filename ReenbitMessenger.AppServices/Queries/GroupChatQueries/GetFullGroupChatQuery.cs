using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
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
