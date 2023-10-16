using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BuberDinner.Api.UnitTests.Consts;
using BuberDinner.Domain.Users;
using BuberDinner.Contracts.Authentication;

namespace BuberDinner.Api.UnitTests
{
    public static class AuthUtils
    {
        public static RegisterRequest CreateRegisterRequest()
        {
            return new RegisterRequest(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
        }

        public static User CreateUser()
        {
            return User.Create(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
        }

        public static LoginRequest CreateLoginRequest()
        {
            return new LoginRequest(Constants.Auth.Email, Constants.Auth.Password);
        }
    }
}
