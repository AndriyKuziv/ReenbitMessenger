using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class DeleteMessageFromGroupChatCommand : ICommand
    {
        public string GroupChatId { get; }
        public long MessageId { get; }

        public DeleteMessageFromGroupChatCommand(string groupChatId, long messageId)
        {
            GroupChatId = groupChatId;
            MessageId = messageId;
        }
    }
}
