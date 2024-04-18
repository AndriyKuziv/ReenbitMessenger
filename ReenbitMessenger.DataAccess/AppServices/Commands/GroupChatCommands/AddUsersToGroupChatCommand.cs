namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class AddUsersToGroupChatCommand : ICommand
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
