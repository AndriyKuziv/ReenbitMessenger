using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.User.Validators
{
    public sealed class EditUserInfoCommandValidator : AbstractValidator<EditUserInfoCommand>
    {
        public EditUserInfoCommandValidator(IUserRepository userRepository)
        {
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
