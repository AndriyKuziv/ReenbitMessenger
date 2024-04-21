using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
{
    public class GetUserMessagesHistoryQuery : IQuery<IEnumerable<GroupChatMessage>>
    {
        public string UserId { get; }

        public GetUserMessagesHistoryQuery(string userId)
        {
            UserId = userId;
        }
    }
}
