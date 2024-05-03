using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class EditGroupChatCommandHandler : ICommandHandler<EditGroupChatCommand, GroupChat>
    {

        private readonly IUnitOfWork _unitOfWork;

        public EditGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChat> Handle(EditGroupChatCommand command)
        {
            var groupChat = await _unitOfWork.GetRepository<IGroupChatRepository>()
                .UpdateAsync(command.GroupChatId, new GroupChat()
                {
                    Name = command.Name
                });

            if (groupChat is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return groupChat;
        }
    }
}
