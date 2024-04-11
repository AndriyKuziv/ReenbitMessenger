using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class GroupChatRepository : IGroupChatRepository
    {
        public Task<IEnumerable<GroupChat>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GroupChat> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GroupChat>> FilterAsync<TKey>(Func<GroupChat, bool> predicate, string orderBy, bool ascending = true, int startAt = 0, int take = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GroupChat>> FindAsync(Expression<Func<GroupChat, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<GroupChat> AddAsync(GroupChat entity)
        {
            throw new NotImplementedException();
        }

        public Task<GroupChat> DeleteAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupChat> UpdateAsync<TId>(TId id, GroupChat entity)
        {
            throw new NotImplementedException();
        }

    }
}
