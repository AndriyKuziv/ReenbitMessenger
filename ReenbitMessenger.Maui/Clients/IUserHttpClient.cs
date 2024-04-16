using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest request);

        Task<User> GetUserAsync(Guid id);

        Task<User> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest);

        Task<string> DeleteUserAsync(Guid id);
    }
}