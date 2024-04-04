using ReenbitMessenger.Library.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<User> GetUserAsync(Guid id);

        Task<string> CreateUserAsync(User user);

        Task<User> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest);

        Task<string> DeleteUserAsync(Guid id);
    }
}