using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using System.Linq.Expressions;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly ChatsDataContext _chatsContext;

        public GroupChatRepository(ChatsDataContext chatsContext)
        {
            _chatsContext = chatsContext;
        }

        public async Task<IEnumerable<GroupChat>> GetAllAsync()
        {
            return _chatsContext.GroupChat.AsQueryable();
        }

        public async Task<GroupChat> GetAsync(string chatId)
        {
            return await _chatsContext.GroupChat
                .FirstOrDefaultAsync(chat =>
                    Convert.ToString(chat.Id).ToLower() == Convert.ToString(chatId).ToLower());
        }

        public async Task<GroupChat> GetFullAsync(Guid chatId)
        {
            return await _chatsContext.GroupChat
                .Include(chat => chat.GroupChatMembers)
                .Include(chat => chat.GroupChatMessages)
                .FirstOrDefaultAsync(chat => 
                    Convert.ToString(chat.Id).ToLower() == Convert.ToString(chatId).ToLower());
        }

        public async Task<IEnumerable<GroupChat>> GetUserChatsAsync(string userId)
        {
            return _chatsContext.GroupChat
                .Include(chat => chat.GroupChatMembers.Where(cmem => cmem.UserId == userId))
                .Where(chat => chat.GroupChatMembers.Any(cmem => cmem.UserId == userId));
        }

        public async Task<IEnumerable<GroupChat>> FilterAsync(Func<GroupChat, bool> predicate, string orderBy = "", bool ascending = true, int startAt = 0, int take = 20)
        {
            var chatProp = typeof(GroupChat).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));

            if (chatProp is null)
            {
                chatProp = typeof(GroupChat).GetProperty("Name");
            }

            var groupChats = _chatsContext.GroupChat.AsQueryable();

            var sorted = ascending ? groupChats.Where(predicate).OrderBy(gc => Convert.ToString(chatProp.GetValue(gc))) :
                groupChats.Where(predicate).OrderByDescending(gc => Convert.ToString(chatProp.GetValue(gc)));

            if (take <= 0)
            {
                return sorted.ToList();
            }

            return sorted.Skip(startAt).Take(take);
        }

        public async Task<IEnumerable<GroupChat>> FindAsync(Expression<Func<GroupChat, bool>> predicate)
        {
            return _chatsContext.GroupChat.Where(predicate);
        }

        public async Task<GroupChat> AddAsync(GroupChat groupChat)
        {
            var result = await _chatsContext.GroupChat.AddAsync(groupChat);

            if (result.Entity is null) return null;

            return groupChat;
        }

        public async Task<GroupChat> DeleteAsync<Guid>(Guid chatId)
        {
            var groupChat = await _chatsContext.GroupChat.FindAsync(Convert.ToString(chatId));
            if (groupChat != null)
            {
                _chatsContext.GroupChat.Remove(groupChat);
          }

            return groupChat;
        }

        public async Task<GroupChat> UpdateAsync<Guid>(Guid chatId, GroupChat entity)
        {
            var groupChat = await _chatsContext.GroupChat.FindAsync(Convert.ToString(chatId));
            if (groupChat is null)
            {
                return null;
            }

            groupChat.Name = entity.Name;

            return groupChat;
        }

        // Members methods
        public async Task<IEnumerable<GroupChatMember>> GetMembersAsync(Guid chatId)
        {
            return await _chatsContext.GroupChatMember.Where(gcm => gcm.GroupChatId == Convert.ToString(chatId)).ToListAsync();
        }

        public async Task<bool> IsInGroupChat(Guid chatId, string userId)
        {
            return _chatsContext.GroupChatMember.Any(cmem => cmem.GroupChatId == Convert.ToString(chatId) && cmem.UserId == userId);
        }

        public async Task<GroupChatMember> AddUserToGroupChatAsync(GroupChatMember member)
        {
            var existingMember = await _chatsContext.GroupChatMember
                .FirstOrDefaultAsync(cmem => cmem.UserId == member.UserId && cmem.GroupChatId == member.GroupChatId);
            if (existingMember != null)
            {
                return existingMember;
            }

            var role = await _chatsContext.GroupChatRole.FirstOrDefaultAsync(role => role.Name == "user");
            if (role is null)
            {
                return null;
            }

            member.GroupChatRoleId = role.Id;

            await _chatsContext.GroupChatMember.AddAsync(member);

            return member;
        }

        public async Task<GroupChatMember> RemoveUserFromGroupChatAsync(Guid chatId, string userId)
        {
            var groupChatMember = await _chatsContext.GroupChatMember
                .FirstOrDefaultAsync(cmem => cmem.GroupChatId == Convert.ToString(chatId) && cmem.UserId == userId);
            if (groupChatMember is null)
            {
                return null;
            }

            _chatsContext.GroupChatMember.Remove(groupChatMember);

            return groupChatMember;
        }

        // Messages methods
        public async Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid chatId)
        {
            return _chatsContext.GroupChatMessage.Where(gcm => gcm.GroupChatId == Convert.ToString(chatId));
        }

        public async Task<GroupChatMessage> CreateGroupChatMessageAsync(GroupChatMessage groupChatMessage)
        {
            groupChatMessage.SentTime = DateTime.Now;
            await _chatsContext.GroupChatMessage.AddAsync(groupChatMessage);

            return groupChatMessage;
        }

        public async Task<GroupChatMessage> DeleteGroupChatMessageAsync(Guid chatId, long messageId)
        {
            var message = await _chatsContext.GroupChatMessage
                .FirstOrDefaultAsync(msg => msg.GroupChatId == Convert.ToString(chatId) && msg.Id == messageId);

            if (message is null)
            {
                return null;
            }

            _chatsContext.GroupChatMessage.Remove(message);

            return message;
        }
    }
}
