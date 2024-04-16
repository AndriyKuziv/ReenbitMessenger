using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands.Validators
{
    public sealed class DeletePrivateMessageCommandValidator : AbstractValidator<DeletePrivateMessageCommand>
    {
        public DeletePrivateMessageCommandValidator(IPrivateMessageRepository privateMessageRepository)
        {
            RuleFor(delcomm => delcomm.MessageId).GreaterThanOrEqualTo(0).WithMessage("Message Id cannot be lesser than 0.");
            RuleFor(delcomm => delcomm.MessageId).MustAsync(async(messageId, _) =>
            {
                return await privateMessageRepository.GetAsync(messageId) != null;
            }).WithMessage("Message must exist.");
        }
    }
}
