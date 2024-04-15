using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage, long>
    {
        Task<IEnumerable<PrivateMessage>> GetPrivateChatAsync(string firstUserId, string secondUserId);
    }
}
