using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands
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
            var result = await _unitOfWork.GetRepository<IPrivateMessageRepository>().UpdateAsync(command.MessageId, new Models.Domain.PrivateMessage
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
