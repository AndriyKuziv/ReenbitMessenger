using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<MessengerDataContext>));

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqlConnection("server=DESKTOP-D015PFU\\SQLEXPRESS;database=TestMessengerDB;Trusted_Connection=true;TrustServerCertificate=True");
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
            });
        }
    }
}
