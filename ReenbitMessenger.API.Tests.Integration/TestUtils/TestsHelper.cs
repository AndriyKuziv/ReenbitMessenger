using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReenbitMessenger.API.Tests.Integration.TestUtils
{
    public static class TestsHelper
    {
        [ExcludeFromCodeCoverage]
        public static string GetValidToken()
        {
            var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
                new JwtSecurityToken(
                    JwtTokenProvider.Issuer,
                    JwtTokenProvider.Issuer,
                    new List<Claim> {
                        new(ClaimTypes.NameIdentifier, "9ce16ede-4614-46d4-a593-fbcfdc6c871c"),
                        new(ClaimTypes.Email, "test@gmail.com")
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: JwtTokenProvider.SigningCredentials
                    )
                );
            return token;
        }

        [ExcludeFromCodeCoverage]
        public static string GetInvalidToken()
        {
            var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
                new JwtSecurityToken(
                    JwtTokenProvider.Issuer,
                    JwtTokenProvider.Issuer,
                    new List<Claim> {
                        new(ClaimTypes.Email, "test@gmail.com")
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: JwtTokenProvider.SigningCredentials
                    )
                );
            return token;
        }
    }
}
