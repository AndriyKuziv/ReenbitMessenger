namespace ReenbitMessenger.AppServices.Commands.PrivateMessageCommands
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
