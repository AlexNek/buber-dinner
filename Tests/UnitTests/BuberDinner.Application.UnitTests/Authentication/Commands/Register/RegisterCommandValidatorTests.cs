using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using FluentValidation;
using Moq;
using System;

using FluentValidation.Results;

using Xunit;
using FluentAssertions;

namespace BuberDinner.Application.UnitTests.Authentication.Commands.Register
{
    public class RegisterCommandValidatorTests
    {
        public RegisterCommandValidatorTests()
        {
        }

        [Fact]
        public void HandleRegisterCommand_WhenRegisterDataIsInvalid_ShouldReturnError()
        {
            // Arrange
            var commandValidator = new RegisterCommandValidator();

            // Act
            RegisterCommand menu = new RegisterCommand("", "", "", "");
            ValidationContext<RegisterCommand> context = new ValidationContext<RegisterCommand>(menu);

            // Act
            var validationResult = commandValidator.Validate(context);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(5);
            var expectedErrorCodes = new[] { "NotEmptyValidator", "NotEmptyValidator", "NotEmptyValidator", "EmailValidator", "NotEmptyValidator" };
            var expectedPropertyNames = new[] { "FirstName", "LastName", "Email", "Email", "Password" };
            for (int i = 0; i < validationResult.Errors.Count; i++)
            {
                validationResult.Errors[i].ErrorCode.Should().Be(expectedErrorCodes[i]);
                validationResult.Errors[i].PropertyName.Should().Be(expectedPropertyNames[i]);
            }
        }
    }
}
