using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class RemoveUsersFromGroupChatCommandHandler : ICommandHandler<RemoveUsersFromGroupChatCommand, IEnumerable<string>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public RemoveUsersFromGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<string>> Handle(RemoveUsersFromGroupChatCommand command)
        {
            var repo = _unitOfWork.GetRepository<IGroupChatRepository>();

            var removedUsersIds = new List<string>();

            foreach (var userId in command.UsersIds)
            {
                var chatMember = await repo.RemoveUserFromGroupChatAsync(command.GroupChatId, userId);

                if (chatMember is null)
                {
                    return null;
                }
                removedUsersIds.Add(chatMember.UserId);
            }

            await _unitOfWork.SaveAsync();

            var membersLeft = (await repo.GetGroupChatMembersAsync(command.GroupChatId)).ToList();

            if (membersLeft is null || !membersLeft.Any())
            {
                await repo.DeleteAsync(command.GroupChatId);
            }

            await _unitOfWork.SaveAsync();

            return removedUsersIds;
        }
    }
}
