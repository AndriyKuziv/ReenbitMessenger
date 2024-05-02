using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReenbitMessenger.API.Tests.Integration.TestUtils
{
    public static class TestsHelper
    {
        [ExcludeFromCodeCoverage]
        public static string GetValidToken(IdentityUser user)
        {
            var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
                new JwtSecurityToken(
                    JwtTokenProvider.Issuer,
                    JwtTokenProvider.Issuer,
                    new List<Claim> {
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Email, user.Email)
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: JwtTokenProvider.SigningCredentials
                    )
                );
            return token;
        }

        [ExcludeFromCodeCoverage]
        public static string GetInvalidToken(IdentityUser user)
        {
            var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
                new JwtSecurityToken(
                    JwtTokenProvider.Issuer,
            JwtTokenProvider.Issuer,
                    new List<Claim> {
                        new(ClaimTypes.Email, user.Email)
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: JwtTokenProvider.SigningCredentials
                    )
                );
            return token;
        }
    }
}
