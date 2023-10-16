using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Users;

using FluentAssertions;

namespace BuberDinner.Api.UnitTests;

public static class AuthExtensions
{
    public static void ValidateCreatedFrom(this AuthenticationResponse authenticationResult, RegisterRequest command, string token)
    {
        authenticationResult.Should().NotBeNull();
        authenticationResult.Token.Should().Be(token);
        authenticationResult.FirstName.Should().Be(command.FirstName);
        authenticationResult.LastName.Should().Be(command.LastName);
        authenticationResult.Email.Should().Be(command.Email);
    }

    public static void ValidateCreatedFrom(this AuthenticationResponse authenticationResponse, LoginRequest loginRequest, string token)
    {
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Token.Should().Be(token);

        authenticationResponse.Email.Should().Be(loginRequest.Email);
    }
}
