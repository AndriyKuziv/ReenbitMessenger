using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.AppServices;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.Commands.User;
using ReenbitMessenger.AppServices.Queries.Auth;
using ReenbitMessenger.AppServices.Queries.User;
using ReenbitMessenger.AppServices.Queries.GroupChatQueries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.AppServices.Queries.PrivateMessageQueries;
using ReenbitMessenger.AppServices.Commands.PrivateMessageCommands;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.API.Controllers;

namespace ReenbitMessenger.API.Services
{
    public static class ApiServices
    {
        public static void AddApiServices(this IServiceCollection services)
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
            services.AddScoped<IQueryHandler<GetGroupChatMessagesQuery, IEnumerable<GroupChatMessage>>,
                GetGroupChatMessagesQueryHandler>();

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
            services.AddScoped<ICommandHandler<RemoveUsersFromGroupChatCommand, IEnumerable<string>>, RemoveUsersFromGroupChatCommandHandler>();
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
