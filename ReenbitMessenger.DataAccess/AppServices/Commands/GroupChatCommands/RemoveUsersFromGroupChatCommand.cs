using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class RemoveUsersFromGroupChatCommand : ICommand<IEnumerable<string>>
    {
        public Guid GroupChatId { get; }
        public IEnumerable<string> UsersIds { get; }

        public RemoveUsersFromGroupChatCommand(Guid groupChatId, IEnumerable<string> usersIds)
        {
            GroupChatId = groupChatId;
            UsersIds = usersIds;
        }
    }
}
