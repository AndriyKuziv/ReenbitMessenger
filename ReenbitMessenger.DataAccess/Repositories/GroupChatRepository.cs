using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            return await _chatsContext.GroupChats.ToListAsync();
        }

        public async Task<GroupChat> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GroupChat>> FilterAsync<TKey>(Func<GroupChat, bool> predicate, string orderBy, bool ascending = true, int startAt = 0, int take = 20)
        {
            var chatProp = typeof(IdentityUser).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
    StringComparison.OrdinalIgnoreCase));

            if (chatProp is null)
            {
                chatProp = typeof(IdentityUser).GetProperty("Name");
            }

            var users = _chatsContext.GroupChats.AsQueryable();

            var sortedList = ascending ? users.Where(predicate).OrderBy(usr => chatProp.GetValue(usr)) :
                users.Where(predicate).OrderByDescending(usr => chatProp.GetValue(usr));

            if (take <= 0)
            {
                return sortedList.ToList();
            }

            return sortedList.Skip(startAt).Take(take).ToList();
        }

        public async Task<IEnumerable<GroupChat>> FindAsync(Expression<Func<GroupChat, bool>> predicate)
        {
            return await _chatsContext.GroupChats.Where(predicate).ToListAsync();
        }

        public async Task<GroupChat> AddAsync(GroupChat entity)
        {
            await _chatsContext.GroupChats.AddAsync(entity);

            return entity;
        }

        public async Task<GroupChat> DeleteAsync<Guid>(Guid id)
        {
            var groupChat = await _chatsContext.GroupChats.FindAsync(id);
            if (groupChat != null)
            {
                _chatsContext.GroupChats.Remove(groupChat);
            }

            return groupChat;
        }

        public async Task<GroupChat> UpdateAsync<Guid>(Guid id, GroupChat entity)
        {
            var groupChat = await _chatsContext.GroupChats.FindAsync(id);
            if (groupChat is null)
            {
                return null;
            }

            groupChat.Name = entity.Name;

            return groupChat;
        }

        public async Task<IEnumerable<GroupChatMember>> GetMembersAsync(Guid id)
        {
            return await _chatsContext.GroupChatMembers.Where(gcm => gcm.GroupChatId == id).ToListAsync();
        }

        public async Task<IEnumerable<GroupChatMessage>> GetMessagesAsync(Guid id)
        {
            return await _chatsContext.GroupChatMessages.Where(gcm => gcm.GroupChatId == id).ToListAsync();
        }
    }
}
