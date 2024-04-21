using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Commands.PrivateMessageCommands
{
    public class EditPrivateMessageCommandHandler : ICommandHandler<EditPrivateMessageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditPrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(EditPrivateMessageCommand command)
        {
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().UpdateAsync(command.MessageId, new PrivateMessage
            {
                Text = command.Text,
            });

            if (result is null)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
