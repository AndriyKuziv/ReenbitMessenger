using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest request);

        Task<User> GetUserAsync(string userId);

        Task<User> EditUserInfoAsync(string userId, EditUserInfoRequest editUserInfoRequest);

        Task<string> DeleteUserAsync(string userId);
    }
}