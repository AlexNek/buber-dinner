using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using FluentValidation;
using Moq;
using System;

using FluentAssertions;

using Xunit;

namespace BuberDinner.Application.UnitTests.Authentication.Queries.Login
{
    public class LoginQueryValidatorTests
    {
        [Fact]
        public void HandleLoginCommand_WhenLoginDataIsInvalid_ShouldReturnError()
        {
            // Arrange
            var validator = new LoginQueryValidator();
            LoginQuery menu = new LoginQuery("", "");
            ValidationContext<LoginQuery> context = new ValidationContext<LoginQuery>(menu);
            // Act
            var validationResult = validator.Validate(context);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(3);
            var expectedErrorCodes = new[] { "NotEmptyValidator", "EmailValidator", "NotEmptyValidator" };
            var expectedPropertyNames = new[] { "Email", "Email", "Password" };
            for (int i = 0; i < validationResult.Errors.Count; i++)
            {
                validationResult.Errors[i].ErrorCode.Should().Be(expectedErrorCodes[i]);
                validationResult.Errors[i].PropertyName.Should().Be(expectedPropertyNames[i]);
            }
        }
    }
}
