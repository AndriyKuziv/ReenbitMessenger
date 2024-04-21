using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using System.Data.Entity;
using LinqKit;
using System.Linq.Expressions;
using System.Reflection;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class PrivateMessageRepository : GenericRepository<PrivateMessage, long>, IPrivateMessageRepository
    {

        public PrivateMessageRepository(MessengerDataContext dbContext) : base(dbContext) { }

        public async new Task<PrivateMessage> AddAsync(PrivateMessage message)
        {
            message.SentTime = DateTime.Now;
            var result = await _dbContext.PrivateMessage.AddAsync(message);

            return result.Entity;
        }

        public async new Task<PrivateMessage> UpdateAsync(long messageId, PrivateMessage message)
        {
            var existingMessage = await _dbContext.PrivateMessage.FindAsync(messageId);

            if (existingMessage is null) return null;

            existingMessage.Text = message.Text;

            return existingMessage;
        }

        public async Task<IEnumerable<PrivateMessage>> FindAsync(string searchValue,
            string orderBy = "", bool ascending = true, int startAt = 0, int take = 20)
        {
            var orderByProp = typeof(PrivateMessage).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));
            if (orderByProp is null)
            {
                orderByProp = typeof(PrivateMessage).GetProperty("Id");
            }

            var users = _dbContext.PrivateMessage
                .Include(msg => msg.SenderUser)
                .Include(msg => msg.ReceiverUser)
                .AsQueryable();

            var props = typeof(PrivateMessage).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Expression<Func<PrivateMessage, bool>> combinedExpression = null;

            foreach (var prop in props)
            {
                Expression<Func<PrivateMessage, bool>> expr = user => Convert.ToString(prop.GetValue(user)).Contains(searchValue);

                if (combinedExpression is null)
                {
                    combinedExpression = expr;
                }
                else
                {
                    combinedExpression = CombineExpressions(combinedExpression, expr);
                }
            }

            var sortedList = ascending ?
                users.AsExpandable().Where(combinedExpression.Compile()).OrderBy(usr => orderByProp.GetValue(usr)) :
                    users.AsExpandable().Where(combinedExpression.Compile()).OrderByDescending(usr => orderByProp.GetValue(usr));

            if (take <= 0)
            {
                return sortedList.AsEnumerable();
            }

            return sortedList.Skip(startAt).Take(take);
        }

        public async Task<IEnumerable<PrivateMessage>> GetPrivateChatAsync(string firstUserId, string secondUserId)
        {
            return _dbContext.PrivateMessage
                .Include(pm=> pm.SenderUser)
                .Include(pm=> pm.ReceiverUser)
                .Where(pm => (pm.SenderUserId == firstUserId && pm.ReceiverUserId == secondUserId) ||
                                                        (pm.SenderUserId == secondUserId && pm.ReceiverUserId == firstUserId));
        }
    }
}
