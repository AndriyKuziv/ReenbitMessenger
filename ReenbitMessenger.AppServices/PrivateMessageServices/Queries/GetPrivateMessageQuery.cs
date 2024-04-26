using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Queries
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
