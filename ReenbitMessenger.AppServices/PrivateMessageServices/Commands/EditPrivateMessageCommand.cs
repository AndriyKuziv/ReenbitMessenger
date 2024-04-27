using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class EditPrivateMessageCommand : ICommand<PrivateMessage>
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
