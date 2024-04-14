using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class RemoveUsersFromGroupChatCommand : ICommand
    {
        public string GroupChatId { get; }
        public IEnumerable<string> UsersIds { get; }

        public RemoveUsersFromGroupChatCommand(string groupChatId, IEnumerable<string> usersIds)
        {
            GroupChatId = groupChatId;
            UsersIds = usersIds;
        }
    }
}
