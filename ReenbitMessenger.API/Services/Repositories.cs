using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.API.Services
{
    public static class Repositories
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupChatRepository, GroupChatRepository>();
            services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        }
    }
}
