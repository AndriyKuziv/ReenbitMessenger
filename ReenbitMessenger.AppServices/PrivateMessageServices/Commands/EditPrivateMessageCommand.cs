namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class EditPrivateMessageCommand : ICommand
    {
        public long MessageId { get; }
        public string Text { get; }

        public EditPrivateMessageCommand(long messageId, string text)
        {
            MessageId = messageId;
            Text = text;
        }
    }
}
