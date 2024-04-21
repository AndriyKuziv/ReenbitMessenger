using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest request);

        Task<User> GetUserAsync(string userId);

        Task<bool> EditUserInfoAsync(EditUserInfoRequest editUserInfoRequest);

        Task<bool> DeleteUserAsync();
    }
}