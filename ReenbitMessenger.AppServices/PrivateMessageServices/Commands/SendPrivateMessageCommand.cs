using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class SendPrivateMessageCommand : ICommand<PrivateMessage>
    {
        public string SenderUserId { get; }
        public string ReceiverUserId { get; }
        public string Text { get; }
        public long? MessageToReplyId { get; }

        public SendPrivateMessageCommand(string senderUserId, string receiverUserId, string text, long? messageToReplyId)
        {
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Text = text;
            MessageToReplyId = messageToReplyId;
        }
    }
}
