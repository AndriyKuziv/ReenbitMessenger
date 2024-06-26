﻿using FluentValidation;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.UserServices.Commands.Validators
{
    public sealed class EditUserInfoCommandValidator : AbstractValidator<EditUserInfoCommand>
    {
        public EditUserInfoCommandValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.UserId).MustAsync(async (userId, _) =>
            {
                return await userRepository.GetAsync(userId) != null;
            });

            RuleFor(edcomm => edcomm.Username)
                .NotEmpty().WithMessage("User name cannot be empty.");

            RuleFor(edcomm => edcomm.Email)
                .EmailAddress().WithMessage("Email address has been entered in wrong format.")
                .NotEmpty().WithMessage("Email cannot be empty.");
            RuleFor(edcomm => edcomm.Email).MustAsync(async (email, _) =>
            {
                return await userRepository.IsEmailUniqueAsync(email);
            }).WithMessage("The email must be unique.");
        }
    }
}
