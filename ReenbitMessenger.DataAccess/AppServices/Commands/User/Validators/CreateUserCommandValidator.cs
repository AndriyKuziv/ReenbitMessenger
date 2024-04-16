using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.User.Validators
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            RuleFor(usr => usr.Username)
                .NotEmpty().WithMessage("User name cannot be empty.");

            RuleFor(usr => usr.Email)
                .EmailAddress().WithMessage("Email address has been entered in wrong format.")
                .NotEmpty().WithMessage("Email cannot be empty.");
            RuleFor(usr => usr.Email).MustAsync(async (email, _) =>
            {
                return await userRepository.IsEmailUniqueAsync(email);
            }).WithMessage("The email must be unique.");

            RuleFor(usr => usr.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(5).WithMessage("Password length must be at least 5.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.\=]+").WithMessage("Password must contain at least one (!? *.=).");
        }
    }
}
