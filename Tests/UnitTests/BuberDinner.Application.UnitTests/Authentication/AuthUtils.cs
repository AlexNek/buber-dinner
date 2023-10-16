using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.UnitTests.Consts;
using BuberDinner.Domain.Users;

namespace BuberDinner.Application.UnitTests.Authentication;

public static class AuthUtils
{
    public static RegisterCommand CreateRegisterCommand()
    {
        return new RegisterCommand(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
    }

    public static User CreateUser()
    {
        return User.Create(Constants.Auth.FirstName, Constants.Auth.LastName, Constants.Auth.Email, Constants.Auth.Password);
    }

    public static LoginQuery CreateLoginQuery()
    {
        return new LoginQuery(Constants.Auth.Email, Constants.Auth.Password);
    }
}