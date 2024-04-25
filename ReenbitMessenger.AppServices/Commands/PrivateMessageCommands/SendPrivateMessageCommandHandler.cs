using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Commands.PrivateMessageCommands
{
    public class SendPrivateMessageCommandHandler : ICommandHandler<SendPrivateMessageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendPrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SendPrivateMessageCommand command)
        {
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().AddAsync(new PrivateMessage
            {
                SenderUserId = command.SenderUserId,
                ReceiverUserId = command.ReceiverUserId,
                Text = command.Text,
                MessageToReplyId = command.MessageToReplyId
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
