using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands
{
    public class SendPrivateMessageCommand : ICommand
    {
        public string SenderUserId { get; }
        public string ReceiverUserId { get; }
        public string Text { get; }
        public long MessageToReplyId { get; }

        public SendPrivateMessageCommand(string senderUserId, string receiverUserId, string text, long messageToReplyId)
        {
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Text = text;
            MessageToReplyId = messageToReplyId;
        }
    }
}
