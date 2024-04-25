using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Commands.PrivateMessageCommands
{
    public class DeletePrivateMessageCommandHandler : ICommandHandler<DeletePrivateMessageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePrivateMessageCommand command)
        {
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().DeleteAsync(command.MessageId);

            if (result is null)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
