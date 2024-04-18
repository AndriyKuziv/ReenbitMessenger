using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class DeleteMessagesFromGroupChatCommand : ICommand
    {
        public Guid GroupChatId { get; }
        public IEnumerable<long> MessagesIds { get; }

        public DeleteMessagesFromGroupChatCommand(Guid groupChatId, IEnumerable<long> messagesIds)
        {
            GroupChatId = groupChatId;
            MessagesIds = messagesIds;
        }
    }
}
