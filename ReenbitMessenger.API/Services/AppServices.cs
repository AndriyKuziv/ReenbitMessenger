using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using ReenbitMessenger.DataAccess.AppServices;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.API.Services
{
    public static class AppServices
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();

            services.AddScoped<IQueryHandler<GetUserByIdQuery, IdentityUser>, GetUserByIdQueryHandler>();
            services.AddScoped<IQueryHandler<LogInQuery, IdentityUser>, LogInQueryHandler>();
            services.AddScoped<IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>, GetUsersQueryHandler>();

            services.AddSingleton<HandlersDispatcher>();
        }
    }
}
