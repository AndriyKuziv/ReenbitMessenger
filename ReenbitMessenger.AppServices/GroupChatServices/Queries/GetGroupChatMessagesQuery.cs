using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetGroupChatMessagesQuery : IQuery<IEnumerable<GroupChatMessage>>
    {
        public Guid GroupChatId { get; }
        public int NumberOfMessages { get; }
        public int Page { get; }
        public string MessageContains { get; }
        public bool Ascending { get; }

        public GetGroupChatMessagesQuery(Guid groupChatId, int numberOfMessages = 20, string messageContains = "", int page = 0,
            bool ascending = true)
        {
            GroupChatId = groupChatId;
            NumberOfMessages = numberOfMessages;
            MessageContains = messageContains;
            Page = page;
            Ascending = ascending;
        }
    }
}
