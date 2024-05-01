using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReenbitMessenger.API.Controllers
{
    public static class ControllerHelper
    {
        public static async Task<string> GetUserId(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity is null)
            {
                return null;
            }

            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return null;
            }

            return userIdClaim.Value;
        }
    }
}
