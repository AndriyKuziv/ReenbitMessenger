using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReenbitMessenger.API.Controllers
{
    public static class ControllerHelper
    {
        public static async Task<string> GetUserId(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
