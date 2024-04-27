using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class EditPrivateMessageCommandHandler : ICommandHandler<EditPrivateMessageCommand, PrivateMessage>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditPrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateMessage> Handle(EditPrivateMessageCommand command)
        {
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().UpdateAsync(command.MessageId, new PrivateMessage
            {
                Text = command.Text,
            });

            if (result is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
