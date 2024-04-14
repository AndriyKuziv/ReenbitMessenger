using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class EditGroupChatCommand : ICommand
    {
        public string GroupChatId { get; }
        public string Name { get; }

        public EditGroupChatCommand(string groupChatId, string name)
        {
            GroupChatId = groupChatId;
            Name = name;
        }
    }
}
