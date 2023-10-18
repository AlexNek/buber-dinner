using BuberDinner.Domain.Users;
using BuberDinner.Infrastructure.UnitTests.Consts;

namespace BuberDinner.Infrastructure.UnitTests
{
    public static class AuthUtils
    {
        //public static RegisterRequest CreateRegisterRequest()
        //{
        //    return new RegisterRequest(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
        //}

        public static User CreateUser()
        {
            return User.Create(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
        }

        //public static LoginRequest CreateLoginRequest()
        //{
        //    return new LoginRequest(Constants.Auth.Email, Constants.Auth.Password);
        //}
    }
}
