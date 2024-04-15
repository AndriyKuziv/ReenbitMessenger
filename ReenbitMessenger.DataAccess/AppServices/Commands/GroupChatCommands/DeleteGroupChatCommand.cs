using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class DeleteGroupChatCommand : ICommand
    {
        public Guid GroupChatId { get; }

        public DeleteGroupChatCommand(Guid groupChatId)
        {
            GroupChatId = groupChatId;
        }
    }
}
