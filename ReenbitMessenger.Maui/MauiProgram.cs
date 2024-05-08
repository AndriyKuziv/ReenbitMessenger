using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using ReenbitMessenger.Maui.Auth;
using ReenbitMessenger.Maui.Clients;
using ReenbitMessenger.Maui.Services;

namespace ReenbitMessenger.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<GroupChatService>();
            builder.Services.AddScoped<VideoCallHubService>();

            builder.Services.AddScoped<ChatHubService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMudServices();

            return builder.Build();
        }
    }
}
