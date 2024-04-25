using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using System.Linq;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class GroupChatRepository : GenericRepository<GroupChat, Guid>, IGroupChatRepository
    {

        public GroupChatRepository(MessengerDataContext dbContext) : base(dbContext){ }

        // Group chats methods
        public async Task<GroupChat> GetFullAsync(Guid chatId)
        {
            return await _dbContext.GroupChat
                .Include(chat => chat.GroupChatMembers)
                    .ThenInclude(cmem => cmem.Role)
                .Include(chat => chat.GroupChatMembers)
                    .ThenInclude(cmem => cmem.User)
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }

        public async Task<IEnumerable<GroupChat>> GetUserChatsAsync(string userId)
        {
            return _dbContext.GroupChat
                .Include(chat => chat.GroupChatMembers.Where(cmem => cmem.UserId == userId))
                .Where(chat => chat.GroupChatMembers.Any(cmem => cmem.UserId == userId));
        }

        public async new Task<GroupChat> UpdateAsync(Guid chatId, GroupChat entity)
        {
            var groupChat = await _dbContext.GroupChat.FindAsync(chatId);
            if (groupChat is null)
            {
                return null;
            }

            groupChat.Name = entity.Name;

            return groupChat;
        }

        // Members methods
        public async Task<GroupChatMember> GetMemberAsync(long memberId)
        {
            return await _dbContext.GroupChatMember
                .Include(cmem => cmem.Role)
                .Include(cmem => cmem.User)
                .FirstOrDefaultAsync(cmem => cmem.Id == memberId);
        }

        public async Task<IEnumerable<GroupChatMember>> FilterMembersAsync(Func<GroupChatMember, bool> predicate)
        {
            return _dbContext.GroupChatMember
                .Include(cmem => cmem.Role)
                .Include(cmem => cmem.User)
                .Where(predicate).AsQueryable();
        }

        public async Task<IEnumerable<GroupChatMember>> GetGroupChatMembersAsync(Guid chatId)
        {
            return _dbContext.GroupChatMember.Where(gcm => gcm.GroupChatId == chatId);
        }

        public async Task<bool> IsInGroupChat(Guid chatId, string userId)
        {
            return _dbContext.GroupChatMember.Any(cmem => cmem.GroupChatId == chatId && cmem.UserId == userId);
        }

        public async Task<GroupChatMember> AddUserToGroupChatAsync(GroupChatMember member)
        {
            var existingMember = await _dbContext.GroupChatMember
                .FirstOrDefaultAsync(cmem => cmem.UserId == member.UserId && cmem.GroupChatId == member.GroupChatId);
            if (existingMember != null)
            {
                return existingMember;
            }

            var role = await _dbContext.GroupChatRole.FirstOrDefaultAsync(role => role.Name == "user");
            if (role is null)
            {
                return null;
            }

            member.GroupChatRoleId = role.Id;

            await _dbContext.GroupChatMember.AddAsync(member);

            return member;
        }

        public async Task<GroupChatMember> RemoveUserFromGroupChatAsync(Guid chatId, string userId)
        {
            var groupChatMember = await _dbContext.GroupChatMember
                .FirstOrDefaultAsync(cmem => cmem.GroupChatId == chatId && cmem.UserId == userId);
            if (groupChatMember is null)
            {
                return null;
            }

            _dbContext.GroupChatMember.Remove(groupChatMember);

            return groupChatMember;
        }

        // Messages methods
        public async Task<GroupChatMessage> GetMessageAsync(long messageId)
        {
            return await _dbContext.GroupChatMessage
                .Include(msg => msg.SenderUser)
                .FirstOrDefaultAsync(msg => msg.Id == messageId);
        }

        public async Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid chatId)
        {
            return _dbContext.GroupChatMessage.Where(gcm => gcm.GroupChatId == chatId);
        }

        public async Task<IEnumerable<GroupChatMessage>> GetMessageHistoryAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user is null)
            {
                return null;
            }

            var res = _dbContext.GroupChat
                .Include(chat => chat.GroupChatMembers)
                .Include(chat => chat.GroupChatMessages)
                .Where(chat => chat.GroupChatMembers.Any(cmem => cmem.UserId == userId))
                .SelectMany(chat => chat.GroupChatMessages)
                .OrderBy(mssg => mssg.SentTime);

            return res;
        }

        public async Task<IEnumerable<GroupChatMessage>> FilterMessagesAsync(Func<GroupChatMessage, bool> predicate,
            int startAt = 0, int take = 20, bool ascending = true)
        {
            var orderByProp = typeof(GroupChatMessage).GetProperty("SentTime");

            var query = _dbContext.Set<GroupChatMessage>()
                .Include(cmem => cmem.SenderUser)
                .AsQueryable();

            var filteredList = query.Where(predicate);

            if (take <= 0)
            {
                return ascending ?
                    filteredList.OrderBy(cmem => orderByProp.GetValue(cmem)).AsEnumerable() :
                    filteredList.OrderByDescending(cmem => orderByProp.GetValue(cmem)).AsEnumerable();
            }

            return ascending ?
                filteredList.OrderByDescending(cmem => orderByProp.GetValue(cmem))
                    .Skip(startAt).Take(take).OrderBy(cmem => orderByProp.GetValue(cmem)) :
                filteredList.OrderBy(cmem => orderByProp.GetValue(cmem))
                    .Skip(startAt).Take(take).OrderByDescending(cmem => orderByProp.GetValue(cmem));
        }

        public async Task<GroupChatMessage> CreateGroupChatMessageAsync(GroupChatMessage groupChatMessage)
        {
            groupChatMessage.SentTime = DateTime.Now;
            await _dbContext.GroupChatMessage.AddAsync(groupChatMessage);

            return groupChatMessage;
        }

        public async Task<GroupChatMessage> DeleteGroupChatMessageAsync(Guid chatId, long messageId)
        {
            var message = await _dbContext.GroupChatMessage
                .FirstOrDefaultAsync(msg => msg.GroupChatId == chatId && msg.Id == messageId);

            if (message is null)
            {
                return null;
            }

            _dbContext.GroupChatMessage.Remove(message);

            return message;
        }
    }
}
