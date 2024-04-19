using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReenbitMessenger.DataAccess.AppServices;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.AppServices.Commands.User;
using ReenbitMessenger.DataAccess.AppServices.Queries.Auth;
using ReenbitMessenger.DataAccess.AppServices.Queries.User;
using ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.AppServices.Queries.PrivateMessageQueries;
using ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;

namespace ReenbitMessenger.API.Services
{
    public static class AppServices
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            // Users queries
            services.AddScoped<IQueryHandler<GetUserByIdQuery, IdentityUser>, GetUserByIdQueryHandler>();
            services.AddScoped<IQueryHandler<LogInQuery, IdentityUser>, LogInQueryHandler>();
            services.AddScoped<IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>, GetUsersQueryHandler>();

            // Group chats queries
            services.AddScoped<IQueryHandler<GetGroupChatsQuery, IEnumerable<GroupChat>>, GetGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetFullGroupChatQuery, GroupChat>, GetFullGroupChatQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserGroupChatsQuery, IEnumerable<GroupChat>>, GetUserGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserMessagesHistoryQuery, IEnumerable<GroupChatMessage>>, GetUserMessagesHistoryQueryHandler>();

            // Private messages queries
            services.AddScoped<IQueryHandler<GetPrivateChatQuery, IEnumerable<PrivateMessage>>,  GetPrivateChatQueryHandler>();
            services.AddScoped<IQueryHandler<GetPrivateMessageQuery, PrivateMessage>, GetPrivateMessageQueryHandler>();

            // Users commands
            services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();

            // Group chats commands
            services.AddScoped<ICommandHandler<CreateGroupChatCommand, GroupChat>, CreateGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteGroupChatCommand>, DeleteGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<EditGroupChatCommand>, EditGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<AddUsersToGroupChatCommand, IEnumerable<GroupChatMember>>, AddUsersToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<RemoveUsersFromGroupChatCommand>, RemoveUsersFromGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<SendMessageToGroupChatCommand, GroupChatMessage>, SendMessageToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteMessagesFromGroupChatCommand>, DeleteMessagesFromGroupChatCommandHandler>();

            // Private messages commands
            services.AddScoped<ICommandHandler<SendPrivateMessageCommand>, SendPrivateMessageCommandHandler>();
            services.AddScoped<ICommandHandler<DeletePrivateMessageCommand>,  DeletePrivateMessageCommandHandler>();
            services.AddScoped<ICommandHandler<EditPrivateMessageCommand>, EditPrivateMessageCommandHandler>();

            services.AddScoped<IValidatorsHandler, ValidatorsHandler>();

            // Singletons
            services.AddSingleton<HandlersDispatcher>();
        }
    }
}
