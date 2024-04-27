using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class DeletePrivateMessageCommand : ICommand<PrivateMessage>
    {
        public long MessageId { get; }

        public DeletePrivateMessageCommand(long messageId)
        {
            MessageId = messageId;
        }
    }
}
