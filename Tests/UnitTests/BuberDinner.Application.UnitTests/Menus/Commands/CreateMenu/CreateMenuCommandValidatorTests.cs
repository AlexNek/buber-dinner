using BuberDinner.Application.Menus.Commands.CreateMenu;
using Moq;
using System;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Xunit;

namespace BuberDinner.Application.UnitTests.Menus.Commands.CreateMenu
{
    public class CreateMenuCommandValidatorTests
    {
        [Fact]
        public void HandleCreateMenu_WhenMenuIsInvalid_ShouldReturnError()
        {
            // Arrange
            var commandValidator = new CreateMenuCommandValidator();

            CreateMenuCommand menu = new CreateMenuCommand(Guid.Empty, "", "", null);
            ValidationContext<CreateMenuCommand> context=new ValidationContext<CreateMenuCommand>(menu);

            // Act
            ValidationResult validationResult = commandValidator.Validate(context);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(3);
            var expectedErrorCodes = new[] { "NotEmptyValidator", "NotEmptyValidator", "NotEmptyValidator" };
            var expectedPropertyNames = new[] { "Name", "Description", "Sections" };
            for (int i = 0; i < validationResult.Errors.Count; i++)
            {
                validationResult.Errors[i].ErrorCode.Should().Be(expectedErrorCodes[i]);
                validationResult.Errors[i].PropertyName.Should().Be(expectedPropertyNames[i]);
            }
        }
    }
}
