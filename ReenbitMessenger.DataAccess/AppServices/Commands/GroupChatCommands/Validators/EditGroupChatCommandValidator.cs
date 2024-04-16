using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
{
    public sealed class EditGroupChatCommandValidator : AbstractValidator<EditGroupChatCommand>
    {
        public EditGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(chat => chat.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group chat must exist");

            RuleFor(chat => chat.Name).NotEmpty();
        }
    }
}
