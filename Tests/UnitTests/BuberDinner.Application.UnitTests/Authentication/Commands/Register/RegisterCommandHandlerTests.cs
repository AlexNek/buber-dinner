using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Users;

using FluentAssertions;

using Moq;
using System.ComponentModel.Design;

using BuberDinner.Application.UnitTests.Consts;
using BuberDinner.Application.UnitTests.Authentication;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.UnitTests.Authentication.Commands.Register
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;

        private readonly MockRepository _mockRepository;

        private readonly Mock<IUserRepository> _mockUserRepository;

        
        public RegisterCommandHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockJwtTokenGenerator = _mockRepository.Create<IJwtTokenGenerator>();
            _mockJwtTokenGenerator.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(Constants.Token.Jwt);

            _mockUserRepository = _mockRepository.Create<IUserRepository>();
            _mockUserRepository.Setup(m => m.Add(It.IsAny<User>()));
        }

        [Fact]
        public async Task HandleRegisterCommand_WhenDataValid_ReturnToken()
        {
            // Arrange
            var registerCommandHandler = CreateRegisterCommandHandler();
            RegisterCommand command = AuthUtils.CreateRegisterCommand();
            CancellationToken cancellationToken = default(CancellationToken);
            _mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns((User)null!);

            // Act
            var result = await registerCommandHandler.Handle(command, cancellationToken);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.ValidateCreatedFrom(command, Constants.Token.Jwt);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task HandleRegisterCommand_WhenUserWithEmailAlreadyExists_ShouldReturnDuplicateEmailError()
        {
            // Arrange
            var registerCommandHandler = CreateRegisterCommandHandler();
            RegisterCommand command = AuthUtils.CreateRegisterCommand(); // replace with appropriate command creation
            CancellationToken cancellationToken = default(CancellationToken);
            _mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(AuthUtils.CreateUser()); // simulate user already exists
            // Act
            var result = await registerCommandHandler.Handle(command, cancellationToken);
            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.DuplicateEmail);
            //_mockRepository.VerifyAll();
        }

        private RegisterCommandHandler CreateRegisterCommandHandler()
        {
            return new RegisterCommandHandler(_mockJwtTokenGenerator.Object, _mockUserRepository.Object);
        }
    }
}
