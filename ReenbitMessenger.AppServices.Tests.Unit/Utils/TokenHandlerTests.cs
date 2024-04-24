using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ReenbitMessenger.AppServices.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReenbitMessenger.AppServices.Tests.Unit.Utils
{
    public class TokenHandlerTests
    {
        private Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();

        [Fact]
        public async Task CreateTokenAsync_ReturnsValidToken()
        {
            // Arrange
            configurationMock.Setup(conf => conf["JwtSettings:Key"]).Returns("c3d456e354f94cd08591bd46b5cd3c1b");
            configurationMock.Setup(conf => conf["JwtSettings:Issuer"]).Returns("issuer");
            configurationMock.Setup(conf => conf["JwtSettings:Audience"]).Returns("audience");

            var tokenHandler = new AppServices.Utils.TokenHandler(configurationMock.Object);
            var user = new IdentityUser
            {
                Email = "test@test.com",
                Id = "12345"
            };

            // Act
            var token = await tokenHandler.CreateTokenAsync(user);

            var jwtHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "issuer",

                ValidateAudience = true,
                ValidAudience = "audience",

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c3d456e354f94cd08591bd46b5cd3c1b")),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            // Assert
            Assert.NotNull(token);

            var principal = jwtHandler.ValidateToken(token, validationParameters, out _);
            var identity = principal.Identity as ClaimsIdentity;

            Assert.NotNull(identity);
            Assert.Equal("test@test.com", identity.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Equal("12345", identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
