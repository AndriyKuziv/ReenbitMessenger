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
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly MessengerDataContext _dbContext;

        public PrivateMessageRepository(MessengerDataContext messengerDataContext)
        {
            _dbContext = messengerDataContext;
        }

        public async Task<IEnumerable<PrivateMessage>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PrivateMessage> GetAsync(long messageId)
        {
            return await _dbContext.PrivateMessage.FindAsync(messageId);
        }

        public async Task<PrivateMessage> AddAsync(PrivateMessage message)
        {
            message.SentTime = DateTime.Now;
            var result = await _dbContext.PrivateMessage.AddAsync(message);

            return result.Entity;
        }

        public async Task<PrivateMessage> DeleteAsync(long messageId)
        {
            var message = await _dbContext.PrivateMessage.FindAsync(messageId);

            if (message is null)
            {
                return null;
            }

            _dbContext.PrivateMessage.Remove(message);

            return message;
        }

        public async Task<PrivateMessage> UpdateAsync(long messageId, PrivateMessage message)
        {
            var existingMessage = await _dbContext.PrivateMessage.FindAsync(messageId);

            if (existingMessage is null) return null;

            existingMessage.Text = message.Text;

            return existingMessage;
        }

        public async Task<IEnumerable<PrivateMessage>> FilterAsync(Func<PrivateMessage, bool> predicate, string orderBy = "", bool ascending = true, int startAt = 0, int take = 20)
        {
            var messageProp = typeof(PrivateMessage).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));

            if (messageProp is null)
            {
                messageProp = typeof(PrivateMessage).GetProperty("SentTime");
            }

            var privateMessages = _dbContext.PrivateMessage.AsQueryable();

            var sorted = ascending ? privateMessages.Where(predicate).OrderBy(pm => Convert.ToString(messageProp.GetValue(pm))) :
                privateMessages.Where(predicate).OrderByDescending(pm => Convert.ToString(messageProp.GetValue(pm)));

            if (take <= 0)
            {
                return sorted;
            }

            return sorted.Skip(startAt).Take(take);
        }

        public async Task<IEnumerable<PrivateMessage>> FindAsync(Expression<Func<PrivateMessage, bool>> predicate)
        {
            return _dbContext.PrivateMessage.Where(predicate);
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
