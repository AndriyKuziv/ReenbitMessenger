using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class AddUsersToGroupChatCommandHandler : ICommandHandler<AddUsersToGroupChatCommand, IEnumerable<GroupChatMember>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUsersToGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChatMember>> Handle(AddUsersToGroupChatCommand command)
        {
            var groupChatRepo = _unitOfWork.GetRepository<IGroupChatRepository>();
            var chatMembersIds = new List<long>();

            foreach (var userId in command.UsersIds)
            {
                var chatMember = await groupChatRepo.AddUserToGroupChatAsync(new GroupChatMember { GroupChatId = command.GroupChatId, UserId = userId });

                if (chatMember is null)
                {
                    return null;
                }

                chatMembersIds.Add(chatMember.Id);
            }

            await _unitOfWork.SaveAsync();

            return (await groupChatRepo.FilterMembersAsync(cmem => cmem.GroupChatId == command.GroupChatId && command.UsersIds.Contains(cmem.UserId))).ToList();
        }
    }
}
