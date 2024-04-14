using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class CreateGroupChatCommand : ICommand
    {
        public string Name { get; }
        public string UserId { get; }

        public CreateGroupChatCommand(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
    }
}
