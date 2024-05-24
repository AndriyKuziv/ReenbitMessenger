using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.AppServices;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.API.Controllers;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.AppServices.AuthServices;
using ReenbitMessenger.AppServices.PrivateMessageServices.Queries;
using ReenbitMessenger.AppServices.UserServices.Queries;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.AppServices.UserServices.Commands;

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
            services.AddScoped<IQueryHandler<GetUserAvatarByIdQuery, string>, GetUserAvatarByIdQueryHandler>();

            // Group chats queries
            services.AddScoped<IQueryHandler<GetGroupChatsQuery, IEnumerable<GroupChat>>, GetGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetGroupChatInfoQuery, GroupChat>, GetGroupChatInfoQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserGroupChatsQuery, IEnumerable<GroupChat>>, GetUserGroupChatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserMessagesHistoryQuery, IEnumerable<GroupChatMessage>>, GetUserMessagesHistoryQueryHandler>();
            services.AddScoped<IQueryHandler<GetGroupChatMessagesQuery, IEnumerable<GroupChatMessage>>,
                GetGroupChatMessagesQueryHandler>();

            // Private messages queries
            services.AddScoped<IQueryHandler<GetPrivateChatQuery, IEnumerable<PrivateMessage>>,  GetPrivateChatQueryHandler>();
            services.AddScoped<IQueryHandler<GetPrivateMessageQuery, PrivateMessage>, GetPrivateMessageQueryHandler>();

            // Users commands
            services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<EditUserInfoCommand, IdentityUser>, EditUserInfoCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
            services.AddScoped<ICommandHandler<UploadUserAvatarCommand, string>, UploadUserAvatarCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteUserAvatarCommand>, DeleteUserAvatarCommandHandler>();

            // Group chats commands
            services.AddScoped<ICommandHandler<CreateGroupChatCommand, GroupChat>, CreateGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteGroupChatCommand>, DeleteGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<EditGroupChatCommand, GroupChat>, EditGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<AddUsersToGroupChatCommand, IEnumerable<GroupChatMember>>, AddUsersToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<RemoveUsersFromGroupChatCommand, IEnumerable<string>>, RemoveUsersFromGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<SendMessageToGroupChatCommand, GroupChatMessage>, SendMessageToGroupChatCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteMessageFromGroupChatCommand, GroupChatMessage>, DeleteMessageFromGroupChatCommandHandler>();

            // Private messages commands
            services.AddScoped<ICommandHandler<SendPrivateMessageCommand, PrivateMessage>, SendPrivateMessageCommandHandler>();
            services.AddScoped<ICommandHandler<DeletePrivateMessageCommand, PrivateMessage>,  DeletePrivateMessageCommandHandler>();
            services.AddScoped<ICommandHandler<EditPrivateMessageCommand, PrivateMessage>, EditPrivateMessageCommandHandler>();

            services.AddScoped<IValidatorsHandler, ValidatorsHandler>();

            // Singletons
            services.AddSingleton<HandlersDispatcher>();
        }
    }
}
