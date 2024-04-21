using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class DeleteGroupChatCommandHandler : ICommandHandler<DeleteGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DeleteGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteGroupChatCommand command)
        {
            var groupChat = await _unitOfWork.GetRepository<IGroupChatRepository>().DeleteAsync(command.GroupChatId);

            if (groupChat is null)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
