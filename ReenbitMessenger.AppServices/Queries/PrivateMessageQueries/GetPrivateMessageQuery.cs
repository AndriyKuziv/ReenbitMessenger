using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Queries.PrivateMessageQueries
{
    public class GetPrivateMessageQuery : IQuery<PrivateMessage>
    {
        public long MessageId { get; }

        public GetPrivateMessageQuery(long messageId)
        {
            MessageId = messageId;
        }
    }
}
