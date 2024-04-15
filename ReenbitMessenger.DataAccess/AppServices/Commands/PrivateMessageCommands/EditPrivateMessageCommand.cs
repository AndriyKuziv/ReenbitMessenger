using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands
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
