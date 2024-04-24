﻿using FluentValidation;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
{
    public class DeleteGroupChatCommandValidator : AbstractValidator<DeleteGroupChatCommand>
    {
        public DeleteGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId)
                .MustAsync(async (gcId, _) =>
                {
                    return await groupChatRepository.GetAsync(gcId) != null;
                }).WithMessage("Group chat must exist.");
        }
    }
}
