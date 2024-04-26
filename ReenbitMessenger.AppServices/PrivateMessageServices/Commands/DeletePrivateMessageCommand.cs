namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class DeletePrivateMessageCommand : ICommand
    {
        public long MessageId { get; }

        public DeletePrivateMessageCommand(long messageId)
        {
            MessageId = messageId;
        }
    }
}
