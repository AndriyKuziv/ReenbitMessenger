using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class DeleteMessageFromGroupChatCommand : ICommand<GroupChatMessage>
    {
        public Guid GroupChatId { get; }
        public string UserId { get; }
        public long MessageId { get; }

        public DeleteMessageFromGroupChatCommand(Guid groupChatId, string userId, long messageId)
        {
            GroupChatId = groupChatId;
            UserId = userId;
            MessageId = messageId;
        }
    }
}
