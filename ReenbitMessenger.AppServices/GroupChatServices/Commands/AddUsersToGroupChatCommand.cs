﻿using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class AddUsersToGroupChatCommand : ICommand<IEnumerable<GroupChatMember>>
    {
        public Guid GroupChatId { get; set; }
        public IEnumerable<string> UsersIds { get; }

        public AddUsersToGroupChatCommand(Guid groupChatId, IEnumerable<string> usersIds)
        {
            GroupChatId = groupChatId;
            UsersIds = usersIds;
        }
    }
}
