using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class LogInQuery : IQuery<IdentityUser>
    {
        public string Username { get; }
        public string Password { get; }

        public LogInQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
