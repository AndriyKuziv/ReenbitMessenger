using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
{
    public class GetUserGroupChatsQuery : IQuery<IEnumerable<GroupChat>>
    {
        public string UserId { get; }

        public GetUserGroupChatsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
