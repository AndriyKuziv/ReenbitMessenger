using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ReenbitMessenger.API.Tests.Integration.TestUtils
{
    public static class JwtTokenProvider
    {
        public static string Issuer { get; } = "Issuer";

        public static SecurityKey SecurityKey { get; } = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Convert.ToString(Guid.NewGuid())));

        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        public static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    }
}
