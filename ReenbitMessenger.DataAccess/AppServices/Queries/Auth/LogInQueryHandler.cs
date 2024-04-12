using AutoMapper;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReenbitMessenger.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.Auth
{
    public class LogInQueryHandler : IQueryHandler<LogInQuery, IdentityUser>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public LogInQueryHandler(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityUser> Handle(LogInQuery query)
        {
            var result = await _signInManager.PasswordSignInAsync(
                query.Username,
                query.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return null;
            }

            return await _userManager.FindByNameAsync(query.Username);
        }
    }
}
