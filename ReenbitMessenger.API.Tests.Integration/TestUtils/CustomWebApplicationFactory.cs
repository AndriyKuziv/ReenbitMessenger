using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReenbitMessenger.DataAccess.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ReenbitMessenger.API.Tests.Integration.TestUtils
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
        TStartup : class
    {
        public string DefaultUserId { get; set; } = "0496b450-020f-4cf7-8c01-10b6b3cfc52e";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<MessengerDataContext>));

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqlConnection("server=DESKTOP-573JB6J\\SQLEXPRESS;database=MessengerDB;Trusted_Connection=true;TrustServerCertificate=True");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<MessengerDataContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlServer(connection);
                }, ServiceLifetime.Singleton);
                services.Configure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                        options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    });
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);
                services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            });
        }
    }
}
