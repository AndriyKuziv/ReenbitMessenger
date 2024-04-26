using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
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
