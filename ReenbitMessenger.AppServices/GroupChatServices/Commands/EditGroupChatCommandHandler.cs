using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class EditGroupChatCommandHandler : ICommandHandler<EditGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public EditGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(EditGroupChatCommand command)
        {
            var groupChat = await _unitOfWork.GetRepository<IGroupChatRepository>()
                .UpdateAsync(command.GroupChatId, new GroupChat()
                {
                    Name = command.Name
                });

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
