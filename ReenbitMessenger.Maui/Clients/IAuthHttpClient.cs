using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IAuthHttpClient
    {
        Task<string> LogInAsync(LoginRequest loginRequest);
        Task<bool> RegisterAsync(CreateUserRequest createUserRequest);
    }
}