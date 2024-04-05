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
        string tokenString = await _localStorage.GetItemAsync<string>("jwtToken");
        if (string.IsNullOrEmpty(tokenString))
        {
            Logout();
        }

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
        }

        currentUser = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(currentUser);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public void Logout()
    {
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentUser)));
    }
}