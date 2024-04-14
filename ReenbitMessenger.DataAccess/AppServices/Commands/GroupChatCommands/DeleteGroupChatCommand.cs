using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class DeleteGroupChatCommand : ICommand
    {
        public string GroupChatId { get; }

        public DeleteGroupChatCommand(string groupChatId)
        {
            GroupChatId = groupChatId;
        }
    }
}
