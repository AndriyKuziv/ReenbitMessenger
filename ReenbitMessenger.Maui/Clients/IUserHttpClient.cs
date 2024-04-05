﻿using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.Maui.Clients
{
    public interface IUserHttpClient
    {
        Task<string> LogInAsync(string email, string password);

        Task<User> GetUserAsync(Guid id);

        Task<bool> RegisterAsync(string email, string password);

        Task<User> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest);

        Task<string> DeleteUserAsync(Guid id);
    }
}