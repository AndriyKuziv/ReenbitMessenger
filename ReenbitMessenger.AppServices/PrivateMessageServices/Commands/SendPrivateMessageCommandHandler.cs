using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands
{
    public class SendPrivateMessageCommandHandler : ICommandHandler<SendPrivateMessageCommand, PrivateMessage>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendPrivateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateMessage> Handle(SendPrivateMessageCommand command)
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
                return null;
            }

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
