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

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class LogInQueryHandler : IQueryHandler<LogInQuery, IdentityUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogInQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IdentityUser> Handle(LogInQuery query)
        {
            var userRepository = (IUserRepository)_unitOfWork.GetRepository<IdentityUser>();

            var user = userRepository.AuthenticateAsync(query.Email, query.Password);

            return user;
        }
    }
}
