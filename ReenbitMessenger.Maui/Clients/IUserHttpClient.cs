using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<string> LogInAsync(LoginRequest loginRequest);

        Task<User> GetUserAsync(Guid id);

        Task<bool> RegisterAsync(CreateUserRequest createUserRequest);

        Task<User> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest);

        Task<string> DeleteUserAsync(Guid id);
    }
}