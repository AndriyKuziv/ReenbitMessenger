using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands
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
