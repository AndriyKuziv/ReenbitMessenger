using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ReenbitMessenger.Maui.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? tokenString = await _localStorage.GetItemAsync<string>("jwt");

        ClaimsIdentity identity;
        if (string.IsNullOrEmpty(tokenString))
        {
            identity = new ClaimsIdentity();
        }
        else
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenString);
            var claims = token.Claims;
            identity = new ClaimsIdentity(claims, "jwt");

            var userId = claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                await _localStorage.SetItemAsStringAsync("userId", userId);
            }
        }

        currentUser = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(currentUser);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
}