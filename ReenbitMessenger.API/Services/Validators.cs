using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.AppServices.Commands.User.Validators;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators;
using ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands.Validators;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess;

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
