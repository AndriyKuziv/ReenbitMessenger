using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class DeletePrivateMessageCommandHandler : ICommandHandler<DeletePrivateMessageCommand, PrivateMessage>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateMessage> Handle(DeletePrivateMessageCommand command)
        {
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().DeleteAsync(command.MessageId);

            if (result is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
