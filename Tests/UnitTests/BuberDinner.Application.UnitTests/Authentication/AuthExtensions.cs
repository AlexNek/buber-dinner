using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Domain.Users;

using FluentAssertions;

namespace BuberDinner.Application.UnitTests.Authentication;

public static class AuthExtensions
{
    public static void ValidateCreatedFrom(this AuthenticationResult authenticationResult, RegisterCommand command, string token)
    {
        authenticationResult.Token.Should().Be(token);
        User user = authenticationResult.User;
        user.Should().NotBeNull();
        user.FirstName.Should().Be(command.FirstName);
        user.LastName.Should().Be(command.LastName);
        user.Email.Should().Be(command.Email);
        bool verified = BCrypt.Net.BCrypt.Verify(command.Password, user.Password.Value);
        verified.Should().BeTrue();
    }

    public static void ValidateCreatedFrom(this AuthenticationResult authenticationResult, LoginQuery query, string token)
    {
        authenticationResult.Token.Should().Be(token);
        User user = authenticationResult.User;
        user.Should().NotBeNull();
        user.Email.Should().Be(query.Email);
        bool verified = BCrypt.Net.BCrypt.Verify(query.Password, user.Password.Value);
        verified.Should().BeTrue();
    }
}
