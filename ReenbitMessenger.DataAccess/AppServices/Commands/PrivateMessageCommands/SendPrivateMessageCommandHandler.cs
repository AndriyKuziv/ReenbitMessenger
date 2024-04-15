using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands
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
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().AddAsync(new Models.Domain.PrivateMessage
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
