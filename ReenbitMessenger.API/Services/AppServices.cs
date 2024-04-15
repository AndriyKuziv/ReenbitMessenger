using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReenbitMessenger.DataAccess.AppServices;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.AppServices.Commands.User;
using ReenbitMessenger.DataAccess.AppServices.Queries.Auth;
using ReenbitMessenger.DataAccess.AppServices.Queries.User;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.API.Services
{
    public static class AppServices
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            // Users commands
            services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();

            // Chats commands
            services.AddScoped<ICommandHandler<CreateGroupChatCommand>, CreateGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteGroupChatCommand>, DeleteGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<EditGroupChatCommand>, EditGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<AddUsersToGroupChatCommand>, AddUsersToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<RemoveUsersFromGroupChatCommand>, RemoveUsersFromGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<SendMessageToGroupChatCommand>, SendMessageToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteMessageFromGroupChatCommand>, DeleteMessageFromGroupChatCommandHandler>();

            // Users queries
            services.AddScoped<IQueryHandler<GetUserByIdQuery, IdentityUser>, GetUserByIdQueryHandler>();
            services.AddScoped<IQueryHandler<LogInQuery, IdentityUser>, LogInQueryHandler>();
            services.AddScoped<IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>, GetUsersQueryHandler>();

            // Chats queries
            services.AddScoped<IQueryHandler<GetGroupChatsQuery, IEnumerable<GroupChat>>, GetGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetFullGroupChatQuery, GroupChat>, GetFullGroupChatQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserGroupChatsQuery, IEnumerable<GroupChat>>, GetUserGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserMessagesHistoryQuery, IEnumerable<GroupChatMessage>>, GetUserMessagesHistoryQueryHandler>();

            services.AddSingleton<HandlersDispatcher>();
        }
    }
}
