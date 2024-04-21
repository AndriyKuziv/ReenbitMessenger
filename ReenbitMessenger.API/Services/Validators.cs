using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using ReenbitMessenger.AppServices.Commands.User.Validators;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands.Validators;
using ReenbitMessenger.AppServices.Commands.PrivateMessageCommands.Validators;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.API.Services
{
    public static class Validators
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        }
    }
}
